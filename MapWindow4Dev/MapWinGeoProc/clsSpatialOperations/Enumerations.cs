//********************************************************************************************************
//File name: Enumerations.cs
//Description: Public class that provides access to all enumerations within MapWinGeoProc.
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
//4-1-06 ah - Angela Hillier - Added buffer enumerations. 							
//********************************************************************************************************
using System;

namespace MapWinGeoProc
{
	/// <summary>
	/// Contains enumerated types used in the MapWinGeoProc library.
	/// </summary>
	public class Enumerations
	{
		/// <summary>
		/// Specify if the buffer shape should have rounded or pointed caps.
		/// </summary>
		public enum Buffer_CapStyle
		{
			/// <summary>
			/// Connecting segments are joined by a single point.
			/// </summary>
			Pointed = 0,
			/// <summary>
			/// Connecting segments are joined by a partial circle.
			/// </summary>
			Rounded = 1
		};
		
		internal enum Buffer_EndCapStyle{Pointed = 0, Rounded = 1, ClosePolygon = 2};
		
		/// <summary>
		/// Specify how much of the line should be buffered.
		/// </summary>
		public enum Buffer_LineSide
		{
			/// <summary>
			/// The buffer will occur on both sides of the line.
			/// </summary>
			Both = 0,
			/// <summary>
			/// The buffer will occur on the left side of the line.
			/// </summary>
			Left = 1,
			/// <summary>
			/// The buffer will occur on the right side of the line.
			/// </summary>
			Right = 2
		};
		
		/// <summary>
		/// Specify how holes in multiPart polygons should be treated.
		/// </summary>
		public enum Buffer_HoleTreatment
		{
			/// <summary>
			/// Holes will not be considered or included in the resulting buffer.
			/// </summary>
			Ignore = 0,
			/// <summary>
			/// Holes will shrink with positive distance and grow with negative distance.
			/// </summary>
			Opposite = 1,
			/// <summary>
			/// Holes will grow with positive distance and shrik with negative distance.
			/// </summary>
			Same = 2,
			/// <summary>
			/// The hole will be included in the result but will not be buffered.
			/// </summary>
			Original = 3
		};
	}
}

