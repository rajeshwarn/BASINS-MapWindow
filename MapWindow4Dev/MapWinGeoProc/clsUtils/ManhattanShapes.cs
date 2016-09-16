//********************************************************************************************************
//File name: ManhattanShapes.cs
//Description: Functions for converting grids to polygon shapes with only 
//horizontal and vertical lines (for speed)
//********************************************************************************************************
//The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
//you may not use this file except in compliance with the License. You may obtain a copy of the License at 
//http://www.mozilla.org/MPL/ 
//Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
//ANY KIND, either express or implied. See the License for the specific language governing rights and 
//limitations under the License. 
//
//The Original Code is MapWindow Open Source. 
//
//Contributor(s): (Open source contributors should list themselves and their modifications here). 
//18/11/08 Chris George original author
//Based on code originally written for the WaterBase project
//********************************************************************************************************


using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Globalization;
using MapWinGIS;

namespace MapWinGeoProc
{
	/// <summary>
	/// <para>ManhattanShapes are polygons with only vertical or horizontal lines in their perimeters.
	/// Lists of ManhattanShapes are the parts of a shape.
	/// Each collection of parts is associated with a particular integer index,
	/// representing a particular grid value.</para>
	/// <para>The algorithm to make the polygons from a grid of cells,
	/// each polygon indexed by the grid value it belongs to is: </para>
	/// <para>1. Make the basic (horizontal) boxes for each index.  
	/// These boxes are unit height and integer width, and located by row and column number,
	/// so that the next two steps only require integer arithmetic.</para>
	/// <para>2. Merge the boxes for each index</para>
	/// <para>3. Make the holes for each index</para>
	/// <para>4. Convert the polygons into lists of points, the format for a shape in a shapefile</para>
	/// Note that grids have the origin at the top left, 
	/// so down direction for example corresponds to an increasing y value, 
	/// and right corresponds to an increasing x value.
	/// </summary>
	public class ManhattanShapes
	{
		/// <summary>
		/// Directions in a cartesian grid.
		/// </summary>
		private enum Direction
		{
			/// <summary>
			/// up
			/// </summary>
			up,
			/// <summary>
			/// right
			/// </summary>
			right,
			/// <summary>
			/// down
			/// </summary>
			down,
			/// <summary>
			/// left
			/// </summary>
			left
		}
		
		/// <summary>
		/// Links are unit length directed lines, positioned in a cartesian grid
		/// using integer coordinates.
		/// Up and down links are positioned by their top ends.
		/// Left and right links are positioned by their left ends.
		/// This means links complement (cancel out when in sequence)
		/// if they have the same position but opposite directions.
		/// </summary>
		private class Link
		{
			/// <summary>
			/// Horizontal coordinate of end of link
			/// </summary>
			public int x;
			/// <summary>
			/// Vertical coordinate of end of link
			/// </summary>
			public int y;
			/// <summary>
			/// Direction of link
			/// </summary>
			public Direction dir;
			
			/// <summary>
			/// Generator
			/// </summary>
			/// <param name="x"></param>
			/// <param name="y"></param>
			/// <param name="dir"></param>
			public Link(int x, int y, Direction dir)
			{
				this.x = x;
				this.y = y;
				this.dir = dir;
			}
			
			/// <summary>
			/// Returns the reverse of dir.
			/// </summary>
			/// <param name="dir"></param>
			/// <returns></returns>
			private static Direction reverse(Direction dir)
			{
				switch (dir)
				{
					case Direction.up:
						return Direction.down;
					case Direction.right:
						return Direction.left;
					case Direction.down:
						return Direction.up;
					default:
						return Direction.right;
				}
			}
			
			/// <summary>
			/// Returns a character indicating the direction
			/// </summary>
			/// <returns></returns>
			public char dc()
			{
				switch (this.dir)
				{
					case Direction.up:
						return 'u';
					case Direction.down:
						return 'd';
					case Direction.left:
						return 'l';
					default:
						return 'r';
				}
			}
			
			/// <summary>
			/// Sets (x,y) to the start point of this link.
			/// </summary>
			/// <param name="x"></param>
			/// <param name="y"></param>
			public void start(out int x, out int y)
			{
				switch (this.dir)
				{
					case Direction.down:
					case Direction.right:
						x = this.x;
						y = this.y;
						break;
					case Direction.up:
						x = this.x;
						y = this.y + 1;
						break;
					default:
						x = this.x + 1;
						y = this.y;
						break;
				}
			}
			
			/// <summary>
			/// Sets (x,y) to the finish point of this link.
			/// </summary>
			/// <param name="x"></param>
			/// <param name="y"></param>
			public void finish(out int x, out int y)
			{
				switch (this.dir)
				{
					case Direction.down:
						x = this.x;
						y = this.y + 1;
						break;
					case Direction.right:
						x = this.x + 1;
						y = this.y;
						break;
					default:
						x = this.x;
						y = this.y;
						break;
				}
			}
			
			/// <summary>
			/// Returns true if the link l complements this link.
			/// </summary>
			/// <param name="l"></param>
			/// <returns></returns>
			public bool complements(Link l)
			{
				return ((l.x == this.x) && (l.y == this.y) && (l.dir == reverse(this.dir)));
			}
			
			/// <summary>
			/// Returns true if the link l starts where this link finishes
			/// </summary>
			/// <param name="l"></param>
			/// <returns></returns>
			public bool continues(Link l)
			{
				int x1, y1, x2, y2;
				this.finish(out x1, out y1);
				l.start(out x2, out y2);
				return ((x1 == x2) && (y1 == y2));
			}
		}
		
		/// <summary>
		/// Bounds of a rectangle that includes the polygon
		/// </summary>
		private class Bounds
		{
			/// <summary>
			/// Minimum horizontal coordinate of bounding rectangle
			/// </summary>
			public int xmin;
			
			/// <summary>
			/// Maximum horizontal coordinate of bounding rectangle
			/// </summary>
			public int xmax;
			
			/// <summary>
			/// Minimum vertical coordinate of bounding rectangle
			/// </summary>
			public int ymin;
			
			/// <summary>
			/// Maximum vertical coordinate of bounding rectangle
			/// </summary>
			public int ymax;
			
			/// <summary>
			/// Constructs a bounding rectangle
			/// </summary>
			/// <param name="xmin"></param>
			/// <param name="xmax"></param>
			/// <param name="ymin"></param>
			/// <param name="ymax"></param>
			public Bounds(int xmin , int xmax, int ymin, int ymax)
			{
				this.xmin = xmin;
				this.xmax = xmax;
				this.ymin = ymin;
				this.ymax = ymax;
			}
			
			/// <summary>
			/// Returns true if no overlap in bounds of this and b
			/// </summary>
			/// <param name="b"></param>
			/// <returns></returns>
			public bool disjoint(Bounds b)
			{
				return ((this.xmin > b.xmax) || (b.xmin > this.xmax) || (this.ymin > b.ymax) || (b.ymin > this.ymax));
			}
		}
		
		/// <summary>
		/// A polygon is stored as a chain of links forming its perimeter, plus its bounds
		/// </summary>
		private class Polygon
		{
			/// <summary>
			/// Perimeter as chain of links
			/// </summary>
			public List<Link> perimeter;
			/// <summary>
			/// Bounding rectangle
			/// </summary>
			public Bounds bounds;
			
			/// <summary>
			/// Constructs a polygon
			/// </summary>
			/// <param name="perimeter"></param>
			/// <param name="bounds"></param>
			public Polygon(List<Link> perimeter, Bounds bounds)
			{
				this.perimeter = perimeter;
				this.bounds = bounds;
			}
			
			/// <summary>
			/// Makes a polygon forming the (clockwise) perimeter of a rectangle
			/// with top left corner (x,y), 1 unit high, and width units wide
			/// </summary>
			/// <param name="x"></param>
			/// <param name="y"></param>
			/// <param name="width"></param>
			/// <returns></returns>
			public static Polygon box(int x, int y, int width)
			{
				List<Link> perimeter = new List<Link>();
				for (int i = 0; i < width; i++)
				{
					perimeter.Add(new Link(x+i, y, Direction.right));
				}
				perimeter.Add(new Link(x+width, y, Direction.down));
				for (int i = width - 1; i >= 0; i--)
				{
					perimeter.Add(new Link(x + i, y+1, Direction.left));
				}
				perimeter.Add(new Link(x,y, Direction.up));
				Bounds bounds = new Bounds(x, x + width, y, y+1);
				Polygon res = new Polygon(perimeter, bounds);
				return res;
			}
			
			/// <summary>
			/// Makes a single polygon from two polygons p1 and p2 with complementary
			/// links at index i1 of perimeter of p1 and i2 of perimeter of p2.
			/// Also removes any adjacent complementary links in the result.
			/// </summary>
			/// <param name="p1"></param>
			/// <param name="i1"></param>
			/// <param name="p2"></param>
			/// <param name="i2"></param>
			public static Polygon merge(Polygon p1, int i1, Polygon p2, int i2)
			{
				List<Link> l1 = p1.perimeter;
				List<Link> l2 = p2.perimeter;
				List<Link> l = l1.GetRange(0, i1);
				l.AddRange(l2.GetRange(i2+1, l2.Count - (i2 + 1)));
				l.AddRange(l2.GetRange(0, i2));
				l.AddRange(l1.GetRange(i1+1, l1.Count - (i1 + 1)));
				// Can get complementary pairs at the joins, so we remove them.
				// Remove pairs at the rightmost join first, so indexes at
				// leftmost are not affected.
				// The guards are the conditions that the rightmost and leftmost
				// portions of l1 respectively are not empty, i.e that there were joins
				// between portions of different lists.  They also ensure the precondition
				// of removePairs(l,i): that i and i+1 are indexes of l.
				if (i1+1 < l1.Count) removePairs(l, i1 + l2.Count - 2);
				if (i1 > 0) removePairs(l, i1 - 1);
				Bounds b1 = p1.bounds;
				Bounds b2 = p2.bounds;
				Bounds bounds =
					new Bounds(
						Math.Min(b1.xmin, b2.xmin),
						Math.Max(b1.xmax, b2.xmax),
						Math.Min(b1.ymin, b2.ymin),
						Math.Max(b1.ymax, b2.ymax));
				Polygon res = new Polygon(l, bounds);
				return res;
			}
			
			/// <summary>
			/// Returns true if p1 and p2 are not disjoint and have complementary links
			/// at indexes i1 and i2 of their perimeters
			/// </summary>
			/// <param name="p1"></param>
			/// <param name="i1"></param>
			/// <param name="p2"></param>
			/// <param name="i2"></param>
			/// <returns></returns>
			public static bool canMerge(Polygon p1, out int i1, Polygon p2, out int i2)
			{
				if (p1.bounds.disjoint(p2.bounds))
				{
					i1 = -1; // for compiler
					i2 = -1; // for compiler
					return false;
				}
				List<Link> l1 = p1.perimeter;
				List<Link> l2 = p2.perimeter;
				for (i1 = 0; i1 < l1.Count; i1++)
				{
					Link link = l1[i1];
					Direction dir = link.dir;
					// only horizontal links need be considered
					if ((dir == Direction.left) || (dir == Direction.right))
					{
						i2 = l2.FindIndex(link.complements);
						if (i2 >= 0) return true;
					}
				}
				i2 = -1; // for compiler
				return false;
			}
			
			/// <summary>
			/// If links at indexes i and i+1 are complementary, removes them.
			/// Recurses on links originally at i-1 and i+2 if i &gt; 0 and i &lt; l.Count - 2
			/// Precondition: 0 &lt;= i &lt; l.Count - 1, ie i and i+1 are indexes of l
			/// </summary>
			/// <param name="l"></param>
			/// <param name="i"></param>
			public static void removePairs(List<Link> l, int i)
			{
				if (l[i].complements(l[i+1]))
				{
					l.RemoveRange(i, 2);
					if ((i > 0) && (i < l.Count)) removePairs(l, i-1);
				}
			}
			
			/// <summary>
			/// Removes the first and last links if they complement, and repeats.
			/// Precondition: l.Count &gt; 1
			/// </summary>
			/// <param name="l"></param>
			public static void removeFirstLast(List<Link> l)
			{
				int last = l.Count - 1;
				if (l[0].complements(l[last]))
				{
					l.RemoveAt(last);
					l.RemoveAt(0);
					if (l.Count > 1) removeFirstLast(l);
				}
			}
			
			/// <summary>
			/// If a chain of links representing a closed polygon
			/// has the first and last links the same direction,
			/// this function puts the head of the chain to the back, and repeats.
			/// This does not affect the polygon, but will reduce by one
			/// the number of points representing it.
			/// </summary>
			/// <param name="l">Chain of links representing a closed polygon</param>
			public static void rotate(List<Link> l)
			{
				Link head = l[0];
				if (head.dir == l[l.Count - 1].dir)
				{
					l.RemoveAt(0);
					l.Add(head);
					rotate(l);
				}
			}
			
			/// <summary>
			/// List l has a link at first continued by a link at last,
			/// i.e. finish of link at first = start of link at last
			/// where first+1 less than last, or first greater than last, with first-last less than l.Count-1 
			/// This removes first+1 to last-1 inclusive
			/// and makes a hole which is the list from first+1 to last-1 inclusive,
			/// where if last less than first this means first+1 to end plus 0 to last-1.
			/// If the complementary links are adjacent the hole will be null.
			/// Note that last may be less than first
			/// </summary>
			/// <param name="l"></param>
			/// <param name="first"></param>
			/// <param name="last"></param>
			/// <param name="hole"></param>
			public static void makeHole(List<Link> l, int first, int last, out List<Link> hole)
			{
				if (first+1 < last)
				{
					hole = l.GetRange(first + 1, last - (first + 1));
					//holes often have complementary initial and final links
					removeFirstLast(hole);
				    l.RemoveRange(first + 1, last - (first + 1));
				}
				else if (first > last)
				{
					hole = new List<Link>();
					if (first < l.Count - 1)
					{
						hole.AddRange(l.GetRange(first+1, l.Count - (first+1)));
						l.RemoveRange(first+1, l.Count - (first+1));
					}
					if (last > 0)
					{
						hole.AddRange(l.GetRange(0, last));
						l.RemoveRange(0, last);
					}
					//holes often have complementary initial and final links
					removeFirstLast(hole);
				}
				else hole = null;
			}
			
			/// <summary>
			/// Returns true if it finds a link continued by a non-adjacent one, 
			/// i.e. finish of link at first = start of link at last
			/// where first+1 less than last or last less than first-1
			/// Then first is set to the index of the first link, and last to
			/// the index of the continuing one.
			/// </summary>
			/// <param name="l"></param>
			/// <param name="first"></param>
			/// <param name="last"></param>
			/// <returns></returns>
			public static bool hasHole(List<Link> l, out int first, out int last)
			{
				for (first = 0; first < l.Count - 1; first++)
				{
					Link link = l[first];
					Direction dir = link.dir;
					last = -1;
					if (first < l.Count - 2)
					  last = l.FindIndex(first+2, link.continues);
					if (last >= 0) return true;
					if (first > 2)
					 last = l.FindIndex(0, first-1, link.continues);
					if (last >= 0) return true;
					
				}
				last = -1;
				return false;
			}
			
			/// <summary>
			/// Generate a string for display of a polygon l, in the the form of
			/// a start point plus a string of direction letters.
			/// This function is intended for debugging.  It also checks the 
			/// polygon is connected and closed.
			/// </summary>
			/// <param name="l"></param>
			/// <returns></returns>
			public static string makeString(List<Link> l)
			{
				if (l.Count < 4) return "Length is " + l.Count.ToString(CultureInfo.CurrentUICulture);
				Link current = l[0];
				Link next;
				int x, y;
				current.start(out x, out y);
				string res = "(" + x.ToString(CultureInfo.CurrentUICulture) + "," +
					y.ToString(CultureInfo.CurrentUICulture) + ") " + current.dc();
				for (int i = 1; i < l.Count; i++)
				{
					next = l[i];
					if (current.continues(next))
					{
						current = next;
						res += current.dc();
					}
					else
					{
						res += " (" +
							current.x.ToString(CultureInfo.CurrentUICulture) + "," +
							current.y.ToString(CultureInfo.CurrentUICulture) + "," + current.dc() +
							") not connected to (" +
							next.x.ToString(CultureInfo.CurrentUICulture) + "," +
							next.y.ToString(CultureInfo.CurrentUICulture) + "," + next.dc() + ")";
						return res;
					}
				}
				next = l[0];
				if (!current.continues(next))
					res += " (" +
						current.x.ToString(CultureInfo.CurrentUICulture) + "," +
						current.y.ToString(CultureInfo.CurrentUICulture) + "," + current.dc() +
						") not connected to start (" +
						next.x.ToString(CultureInfo.CurrentUICulture) + "," +
						next.y.ToString(CultureInfo.CurrentUICulture) + "," + next.dc() + ")";
				return res;
			}
		}
		
		private class Data
		{
			public List<Polygon> polygons;
			/// <summary>
			/// area is number of cells, since grid considered to be unit squares
			/// </summary>
			public int area;
			
			public Data(List<Polygon> polygons, int area)
			{
				this.polygons = polygons;
				this.area = area;
			}
		}
		
		private Dictionary<int, Data> ShapesTable;
		
		private OffSet offset;
		
		private string lastError = "";
		
		/// <summary>
		/// Constructor, making an empty dictionary, and setting offset
		/// from header.
		/// </summary>
		public ManhattanShapes(MapWinGIS.GridHeader h)
		{
			ShapesTable = new Dictionary<int, Data>();
			offset = new OffSet(h);
		}
		
		/// <summary>
		/// Constructor, making an empty dictionary, and setting offset
		/// from origin and cell dimensions
		/// </summary>
		public ManhattanShapes(MapWinGIS.Point p, double dX, double dY)
		{
			ShapesTable = new Dictionary<int, Data>();
			offset = new OffSet(p, dX, dY);
		}
		
		/// <summary>
		/// Cell count for grid value val
		/// </summary>
		/// <param name="val"></param>
		/// <returns>0 if val not found</returns>
		public int CellCount(int val)
		{
			Data data;
			ShapesTable.TryGetValue(val, out data);
			return data.area;
		}
		
		/// <summary>
		/// Area (in square meters if cell dimensions in meters) for grid value val
		/// </summary>
		/// <param name="val"></param>
		/// <returns>0 if val not found</returns>
		public double Area(int val)
		{
			Data data;
			ShapesTable.TryGetValue(val, out data);
			return offset.area(data.area);
		}
		
		/// <summary>
		/// Adds a part p with index indx.
		/// </summary>
		/// <param name="indx"></param>
		/// <param name="p"></param>
		/// <param name="area"></param>
		private void addShape(int indx, Polygon p, int area)
		{
			if (ShapesTable.ContainsKey(indx))
			{
				ShapesTable[indx].polygons.Add(p);
				ShapesTable[indx].area += area;
			}
			else
			{
				List<Polygon> lp = new List<Polygon>();
				lp.Add(p);
				ShapesTable.Add(indx, new Data(lp, area));
			}
		}
		
		/// <summary>
		/// For each index in the dictionary, merges its parts if possible.
		/// </summary>
		private void mergeShapes()
		{
			List<int> keys = new List<int>();
			keys.AddRange(ShapesTable.Keys);
			foreach (int i in keys)
			{
				ShapesTable[i].polygons = mergePolygons(new List<Polygon>(), ShapesTable[i].polygons);
			}
		}
		
		/// <summary>
		/// Merges the polygons in a list todo of polygons.
		/// Two polygons can be merged if they are not disjoint and contain complementary links.
		/// </summary>
		/// <param name="done"></param>
		/// <param name="todo"></param>
		/// <returns></returns>
		private static List<Polygon> mergePolygons(List<Polygon> done, List<Polygon> todo)
		{
			while (todo.Count > 0)
			{
				Polygon p0 = todo[0];
				todo.RemoveAt(0);
				int i0, i1;
				bool changed = false;
				int count = todo.Count;
				for (int i = 0; i < count; i++)
				{
					if (Polygon.canMerge(p0, out i0, todo[i], out i1))
					{
						Polygon p = Polygon.merge(p0, i0, todo[i], i1);
						todo.RemoveAt(i);
						todo.Add(p);
						changed = true;
						break;
					}
				}
				if (!changed) done.Add(p0);
			}
			return done;
		}
		
		/// <summary>
		/// For each index in the dictionary, separates out any holes.
		/// </summary>
		private void makeHoles()
		{
			List<int> keys = new List<int>();
			keys.AddRange(ShapesTable.Keys);
			foreach (int i in keys)
			{
				ShapesTable[i].polygons = makeHoles(new List<Polygon>(), ShapesTable[i].polygons);
			}
		}
		
		/// <summary>
		/// Separates out the holes in a list of polygons.
		/// There may be more than one hole in a polygon, and holes may be split into
		/// several holes.  A polygon contains a hole if it contains two non-adjacent
		/// complementary links.
		/// </summary>
		/// <param name="done">polygons with no holes</param>
		/// <param name="todo">polygons which may have holes</param>
		/// <returns></returns>
		private static List<Polygon> makeHoles(List<Polygon> done, List<Polygon> todo)
		{
			while (todo.Count > 0)
			{
				int first, last;
				Polygon next = todo[0];
				if (Polygon.hasHole(next.perimeter, out first, out last))
				{
					//Debug.WriteLine(Link.makeString(next));
					//Debug.WriteLine("has hole from " + first.ToString() + " to " + last.ToString());
					List<Link> hole;
					Polygon.makeHole(next.perimeter, first, last, out hole);
					if ((hole != null) && (hole.Count > 0))
					{
						// Holes are never merged, so bounds are of no interest
						Bounds bounds = new Bounds(0,0,0,0);
						Polygon p = new Polygon(hole, bounds);
						todo.Add(p);
					}
					if (next.perimeter.Count == 0)
					{
						// degenerate case: all of next was a hole; the empty polygon can be removed
						todo.RemoveAt(0);
					}
				}
				else
				{
					done.Add(next);
					todo.RemoveAt(0);
				}
			}
			return done;
		}
		
		/// <summary>
		/// Finish by merging shapes and making holes
		/// </summary>
		public void FinishShapes()
		{
			mergeShapes();
			makeHoles();
		}
		
		/// <summary>
		/// Generate a string for all the polygons.  For each grid value:
		/// 1.  A line stating its value
		/// 2.  A set of lines, one for each polygon for that value.
		/// </summary>
		/// <returns></returns>
		public string makeString()
		{
			string res = "";
			foreach (KeyValuePair<int, Data> kvp in ShapesTable)
			{
				int indx = kvp.Key;
				List<Polygon> lp = kvp.Value.polygons;
				res += "Index " + indx.ToString(CultureInfo.CurrentUICulture) + '\n';
				for (int i = 0; i < lp.Count; i++)
				{
					res += Polygon.makeString(lp[i].perimeter);
					res += '\n';
				}
			}
			return res;
		}
		
		delegate int Row(int index);
		
		class ArrayRow
		{
			int[] row;
			
			public ArrayRow(int[] row)
			{
				this.row = row;
			}
			
			public int Row(int index)
			{
				return row[index];
			}
		}
		
		class GridRow
		{
			MapWinGIS.Grid grid;
			
			int rowNum;
			
			public bool readError = false;
			
			public GridRow(MapWinGIS.Grid grid, int rowNum)
			{
				this.grid = grid;
				this.rowNum = rowNum;
			}
			
			public int Row(int col)
			{
				try
				{
					return System.Convert.ToInt32(grid.get_Value(col, rowNum), CultureInfo.InvariantCulture);
				}
				catch (InvalidCastException)
				{
					readError = true;
					return 0;
				}
			}
		}
		
		/// <summary>
		/// Adds boxes from array row number rowNum
		/// </summary>
		/// <param name="row"></param>
		/// <param name="rowNum"></param>
		/// <param name="noData"></param>
		public void addArrayRow(int[] row, int rowNum, int noData)
		{
			ArrayRow ar = new ArrayRow(row);
			addRow(new Row(ar.Row), rowNum, row.Length, noData);
		}
		
		/// <summary>
		/// Adds boxes from grid row number rowNum of length length
		/// </summary>
		/// <param name="grid"></param>
		/// <param name="rowNum"></param>
		/// <param name="length"></param>
		public void addGridRow(MapWinGIS.Grid grid, int rowNum, int length)
		{
			GridRow gr = new GridRow(grid, rowNum);
			addRow(new Row(gr.Row), rowNum, length, (int) ((double) grid.Header.NodataValue));
			if (gr.readError) lastError = "Grid does not seem to be an integer grid";
		}
		
		/// <summary>
		/// row behaves like an array, indexed from 0 to length - 1
		/// This creates boxes, where boxes are made from adjacent cells
		/// of the array with the same values, and adds them as parts.
		/// Nodata values are ignored.
		/// </summary>
		/// <param name="row"></param>
		/// <param name="rowNum"></param>
		/// <param name="length"></param>
		/// <param name="noData"></param>
		private void addRow(Row row, int rowNum, int length, int noData)
		{
			int col = 0;
			int width = 1;
			int last = row(0);
			int bound = length - 1;
			while (col < bound)
			{
				int next = row(col+1);
				if (next == last)
				{
					width++;
				}
				else
				{
					if (last != noData) this.addShape(last, Polygon.box(col + 1 - width, rowNum, width), width);
					last = next;
					width = 1;
				}
				col++;
			}
			if (last != noData) this.addShape(last, Polygon.box(col + 1 - width, rowNum, width), width);
		}
		
		/// <summary>
		/// Creates and adds shape corresponding to grid value val to shapeFile
		/// </summary>
		/// <param name="shapeFile"></param>
		/// <param name="val"></param>
		/// <param name="shapeIndex">set to index of added shape</param>
		/// <param name="cb">Callback for reporting errors</param> 
		/// <returns>true if shape successfully added</returns>
		public bool InsertShape(MapWinGIS.Shapefile shapeFile, int val, ref int shapeIndex, MapWinGIS.ICallback cb)
		{
			List<Polygon> lp = ShapesTable[val].polygons;
			MapWinGIS.Shape shape = offset.makeShape(lp);
			if (shape == null) return false;
//			else if (cb != null)
//			{
//				if (!shape.IsValid)
//				{
//					string res = "\nIndex " + val.ToString(CultureInfo.CurrentUICulture) + '\n';
//					for (int i = 0; i < lp.Count; i++)
//					{
//						res += Polygon.makeString(lp[i].perimeter);
//						res += '\n';
//					}
//					cb.Error("ManhattanShapes.makeShapefile", "Invalid shape generated: " + shape.IsValidReason + ": " + res);
//				}
//			}
			return shapeFile.EditInsertShape(shape, ref shapeIndex);
		}
		
		/// <summary>
		/// Create shapefile named shapeName  Any existing shapefile of the same name is deleted.
		/// </summary>
		/// <param name="shapeName">Path of output shapefile</param>
		/// <param name="cb">Callback for reporting errors</param>
		/// <returns>null if any error, else shapefile</returns>
		public MapWinGIS.Shapefile makeShapefile(string shapeName, MapWinGIS.ICallback cb)
		{
			MapWinGeoProc.DataManagement.DeleteShapefile(ref shapeName);
			MapWinGIS.Shapefile shpfile = new MapWinGIS.Shapefile();
			if (!shpfile.CreateNew(shapeName, ShpfileType.SHP_POLYGON))
			{
				if (cb != null) cb.Error("ManhattanShapes.makeShapefile", clsUtils.ManhattanShapes.nocreateshapefile + shapeName);
				return null;
			}
			if (!shpfile.StartEditingShapes(true, null))
			{
				if (cb != null) cb.Error("ManhattanShapes.makeShapefile", clsUtils.ManhattanShapes.noeditshapefile + shapeName);
				return null;
			}
			int shapeindex = 0;
			foreach (int k in ShapesTable.Keys)
			{
				if (!InsertShape(shpfile, k, ref shapeindex, cb))
				{
					if (cb != null) cb.Error("ManhattanShapes.makeShapefile", clsUtils.ManhattanShapes.noaddshape + shapeName);
					shpfile.StopEditingShapes(true, true, null);
					shpfile.Close();
					return null;
				}
			}
			if (!shpfile.StopEditingShapes(true, true, null))
			{
				if (cb != null) cb.Error("ManhattanShapes.makeShapefile", clsUtils.ManhattanShapes.noeditshapefile + shapeName);
				return null;
			}
			if (!shpfile.Close())
			{
				if (cb != null) cb.Error("ManhattanShapes.makeShapefile", clsUtils.ManhattanShapes.nocloseshapefile + shapeName);
				return null;
			}
			return shpfile;
		}
		
		/// <summary>
		/// Converts grid to a shapefile, removing any existing shapefile.
		/// Assumed to be an integer grid.  Adds attribute headed id with the grid values, and
		/// attribute headed "Area" with the area for each polygon.
		/// </summary>
		/// <param name="gridName">Path of input grid</param>
		/// <param name="shapeName">Path of output shapefile</param>
		/// <param name="id">String to use for name of grid values attribute</param>
		/// <param name="cb">Callback for reporting errors</param>
		/// <returns>null if any errors, else shapefile</returns>
		public static MapWinGIS.Shapefile GridToShapeManhattan(string gridName, string shapeName, 
		                                                       string id, MapWinGIS.ICallback cb)
		{
			if (!System.IO.File.Exists(gridName)) 
			{
				if (cb != null) cb.Error("GridToShapeManhattan", clsUtils.ManhattanShapes.nogrid + gridName);
				return null;
			}
			MapWinGIS.Grid grid = new MapWinGIS.GridClass();
			if (!grid.Open(gridName, GridDataType.UnknownDataType, true, GridFileType.UseExtension, null))
			{
				if (cb != null) cb.Error("GridToShapeManhattan", clsUtils.ManhattanShapes.noopengrid + gridName + ": " + grid.get_ErrorMsg(grid.LastErrorCode));
				return null;
			}
			MapWinGIS.GridHeader header = grid.Header;
			int numRows = header.NumberRows;
			int numCols = header.NumberCols;
			MapWinGeoProc.DataManagement.DeleteShapefile(ref shapeName);
			MapWinGIS.Shapefile shapeFile = new MapWinGIS.Shapefile();
			bool success = shapeFile.CreateNew(shapeName, ShpfileType.SHP_POLYGON);
			if (!success)
			{
				if (cb != null) cb.Error("GridToShapeManhattan", clsUtils.ManhattanShapes.nocreateshapefile + shapeName + ": " + shapeFile.get_ErrorMsg(shapeFile.LastErrorCode));
				return null;
			}
			success = shapeFile.StartEditingShapes(true, null);
			if (!success)
			{
				if (cb != null) cb.Error("GridToShapeManhattan", clsUtils.ManhattanShapes.noeditshapefile + shapeName + ": " + shapeFile.get_ErrorMsg(shapeFile.LastErrorCode));
				return null;
			}
			ManhattanShapes shapes = new ManhattanShapes(header);
			for (int i = 0; i < numRows; i++)
				shapes.addGridRow(grid, i, numCols);
			if (!shapes.lastError.Equals(""))
			{
				if (cb != null) cb.Error("GridToShapeManhattan", shapes.lastError);
				return null;
			}
			grid.Close();
			shapes.FinishShapes();
			MapWinGIS.Field idField = new MapWinGIS.Field();
			idField.Name = id;
			idField.Type = FieldType.INTEGER_FIELD;
			int idIndex = 0;
			success = shapeFile.EditInsertField(idField, ref idIndex, null);
			if (!success)
			{
				if (cb != null) cb.Error("GridToShapeManhattan", clsUtils.ManhattanShapes.noaddfield + shapeName + ": " + shapeFile.get_ErrorMsg(shapeFile.LastErrorCode));
				shapeFile.Close();
				return null;
			}
			MapWinGIS.Field areaField = new MapWinGIS.Field();
			areaField.Name = "Area";
			areaField.Type = FieldType.DOUBLE_FIELD;
			int areaIndex = 1;
			success = shapeFile.EditInsertField(areaField, ref areaIndex, null);
			if (!success)
			{
				if (cb != null) cb.Error("GridToShapeManhattan", clsUtils.ManhattanShapes.noaddfield + shapeName + ": " + shapeFile.get_ErrorMsg(shapeFile.LastErrorCode));
				shapeFile.Close();
				return null;
			}
			int shapeIndex = 0;
			foreach (int k in shapes.ShapesTable.Keys)
			{
				success = shapes.InsertShape(shapeFile, k, ref shapeIndex, cb);
				if (!success)
				{
					if (cb != null) cb.Error("GridToShapeManhattan", clsUtils.ManhattanShapes.noaddshape + shapeName + ": " + shapeFile.get_ErrorMsg(shapeFile.LastErrorCode));
					shapeFile.Close();
					return null;
				}
				success = shapeFile.EditCellValue(idIndex, shapeIndex, k);
				if (!success)
				{
					if (cb != null) cb.Error("GridToShapeManhattan", clsUtils.ManhattanShapes.noeditcell + shapeName + ": " + shapeFile.get_ErrorMsg(shapeFile.LastErrorCode));
					shapeFile.Close();
					return null;
				}
				double area = shapes.Area(k);
				success = shapeFile.EditCellValue(areaIndex, shapeIndex, area);
				if (!success)
				{
					if (cb != null) cb.Error("GridToShapeManhattan", clsUtils.ManhattanShapes.noeditcell + shapeName + ": " + shapeFile.get_ErrorMsg(shapeFile.LastErrorCode));
					shapeFile.Close();
					return null;
				}
			}
			success = shapeFile.StopEditingShapes(true, true, null);
			if (!success)
			{
				if (cb != null) cb.Error("GridToShapeManhattan", clsUtils.ManhattanShapes.noeditshapefile + shapeName + ": " + shapeFile.get_ErrorMsg(shapeFile.LastErrorCode));
				shapeFile.Close();
				return null;
			}
			if (!shapeFile.Close())
			{
				if (cb != null) cb.Error("GridToShapeManhattan", clsUtils.ManhattanShapes.nocloseshapefile + shapeName + ": " + shapeFile.get_ErrorMsg(shapeFile.LastErrorCode));
				return null;
			}
			string oldProj = System.IO.Path.ChangeExtension(gridName, ".prj");
			string newProj = System.IO.Path.ChangeExtension(shapeName, ".prj");
			if (System.IO.File.Exists(oldProj) && !oldProj.Equals(newProj)) 
				System.IO.File.Copy(oldProj, newProj, true);
			return shapeFile;
		}
		
		/// <summary>
		/// Stores the values from a grid header used to convert link positions to grid points
		/// </summary>
		private class OffSet
		{
			/// <summary>
			/// Point at top left edge of grid
			/// </summary>
			private MapWinGIS.Point origin;
			
			/// <summary>
			/// E-W distance between grid points
			/// </summary>
			private double dX;
			
			/// <summary>
			/// N-S distance between grid points
			/// </summary>
			private double dY;
			
			/// <summary>
			/// Area of cell
			/// </summary>
			private double unitArea;
			
			/// <summary>
			/// Constructor from grid header
			/// </summary>
			/// <param name="h">header</param>
			public OffSet(MapWinGIS.GridHeader h)
			{
				// origin (taken as column zero and row zero) is at the top left of the grid
				this.origin = new MapWinGIS.Point();
				this.origin.x = h.XllCenter - (h.dX / 2);
				this.origin.y = h.YllCenter - (h.dY / 2) + h.dY * h.NumberRows;
				this.dX = h.dX;
				this.dY = h.dY;
				this.unitArea = (double)this.dX * this.dY;
			}
			
			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="origin"></param>
			/// <param name="dX"></param>
			/// <param name="dY"></param>
			public OffSet(MapWinGIS.Point origin, double dX, double dY)
			{
				this.origin = origin;
				this.dX = dX;
				this.dY = dY;
				this.unitArea = (double)this.dX * this.dY;
			}
			
			/// <summary>
			/// Generate a point from a link's start position
			/// </summary>
			/// <param name="l">link</param>
			/// <returns>point</returns>
			private MapWinGIS.Point linkToPoint(Link l)
			{
				MapWinGIS.Point p = new MapWinGIS.Point();
				int x, y;
				l.start(out x, out y);
				p.x = origin.x + dX * x;
				p.y = origin.y - dY * y;
				return p;
			}
			
			/// <summary>
			/// Converts a count c of unit boxes to an area
			/// </summary>
			/// <param name="c"></param>
			/// <returns>area</returns>
			public double area(int c)
			{
				return c * unitArea;
			}
			
			/// <summary>
			/// Adds a chain of links as a part to a shape
			/// </summary>
			/// <param name="l">chain</param>
			/// <param name="shape"></param>
			/// <param name="partindex"></param>
			/// <param name="pointindex"></param>
			/// <returns>true iff no eror</returns>
			private bool addChainToShape(List<Link> l, MapWinGIS.Shape shape,
			                             ref int partindex, ref int pointindex)
			{
				if (!shape.InsertPart(pointindex, ref partindex)) return false;
				partindex++;
				Polygon.rotate(l);
				Link l0 = l[0];
				MapWinGIS.Point p0;
				p0 = linkToPoint(l0);
				if (!shape.InsertPoint(p0, ref pointindex)) return false;
				pointindex++;
				Direction lastDir = l0.dir;
				for (int i = 1; i < l.Count; i++)
				{
					Link nextLink = l[i];
					if (nextLink.dir != lastDir)
					{
						// next link has a new direction, so include its start point
						if (!shape.InsertPoint(linkToPoint(nextLink), ref pointindex)) return false;
						pointindex++;
					}
					lastDir = nextLink.dir;
				}
				// close the polygon
				if (!shape.InsertPoint(p0, ref pointindex)) return false;
				pointindex++;
				return true;
			}
			
			/// <summary>
			/// Makes a shape from a list of polygons; each polygon becames a part of the shape.
			/// </summary>
			/// <param name="polygons"></param>
			/// <returns>null if error, else shape</returns>
			public MapWinGIS.Shape makeShape(List<Polygon> polygons)
			{
				MapWinGIS.Shape shape = new MapWinGIS.Shape();
				if (!shape.Create(MapWinGIS.ShpfileType.SHP_POLYGON)) return null;
				int pointindex = 0;
				int partindex = 0;
				for (int i = 0; i < polygons.Count; i++)
				{
					if (!addChainToShape(polygons[i].perimeter, shape, ref partindex, ref pointindex)) return null;
				}
				return shape;
			}
		}
	}
}

