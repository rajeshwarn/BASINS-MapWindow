//********************************************************************************************************
//The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
//you may not use this file except in compliance with the License. You may obtain a copy of the License at 
//http://www.mozilla.org/MPL/ 
//Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
//ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
//limitations under the License. 
//
//The Original Code is MapWindow Open Source. 
//
//The Initial Developer of this version of the Original Code is Daniel P. Ames using portions created by 
//Utah State University and the Idaho National Engineering and Environmental Lab that were released as 
//public domain in March 2004.  
//
//Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************
using System.Drawing;
using System;

internal class globals
{
	//public static MapWinGIS.Map map = null;
	//public static LegendControl.Legend Legend = null;
	public static string LastError;

	private struct POINTAPI
	{
		public int x;
		public int y;
	}
   
	[System.Runtime.InteropServices.DllImport("user32.dll")]
	private static extern void GetCursorPos (ref POINTAPI lpPoint);

	public static Point GetCursorLocation()
	{
		POINTAPI pnt = new POINTAPI();
		GetCursorPos(ref pnt);
		return new Point(pnt.x, pnt.y);
	}

	public static bool IsSupportedPicture(object picture)
	{
		if(picture == null)
			return true;

		System.Type picType = picture.GetType();
		if(typeof(Icon) == picType)
			return true;
		if(typeof(Image) == picType)
			return true;
		if(typeof(Bitmap) == picType)
			return true;

		return false;
	}

	public static Color UintToColor(uint val)
	{
		int r,g,b;
		
		GetRGB((int)val,out r, out g, out b);
        		
		return Color.FromArgb(255,r,g,b);
	}

	public static void GetRGB(int Color,out int r, out int  g, out int  b)
	{
		if (Color < 0)
			Color = 0;

		r = (int)(Color & 0xFF);
		g = (int)(Color & 0xFF00) / 256;	//shift right 8 bits
		b = (int)(Color & 0xFF0000) / 65536; //shift right 16 bits
	}

	public static int ColorToInt(Color c)
	{
		int retval = ((int)c.B) << 16;
		retval += ((int)c.G) << 8;
		return retval + ((int)c.R);
	}

//	public static Color HSLtoColor(float Hue, float Sat, float Lum)
//	{
//		//Note: Hue,Sat, Lum are values 0 - 1
//		int r = 0 ,g = 0,b = 0;
//		if( Sat == 0)
//		{
//			r = g = b = (int)(255 * Lum);
//			return Color.FromArgb(r,g,b);
//		}
//
//		float temp1,temp2, rTemp, gTemp, bTemp;
//
//		if(Lum < .5)
//		{
//			temp2=(float)(Lum*(1.0+Sat));
//		}
//		else
//		{
//			temp2=Lum+Sat - Lum*Sat;
//		}
//
//		temp1 = (float)(2.0*Lum - temp2);
//
//		rTemp = Hue+1f/3f;
//		gTemp = Hue;
//		bTemp = Hue - 1f/3f;
//
//		if(rTemp < 0)
//			rTemp += 1;
//		if(gTemp < 0)
//			gTemp += 1;
//		if(bTemp < 0)
//			bTemp += 1;
//
//		//calculate the Red Component
//		if (6.0*rTemp < 1)
//			r = (int)((temp1+(temp2-temp1)*6.0*rTemp) * 255);
//        else if (2.0*rTemp < 1)
//			r = (int)(temp2*255);
//        else //if (3.0*rTemp < 2)
//			r = (int)((temp1+(temp2-temp1)*((2.0/3.0)-rTemp)*6.0)*255);
//
//		//calculate the Green Component
//		if (6.0*gTemp < 1)
//			g = (int)((temp1+(temp2-temp1)*6.0*gTemp) * 255);
//		else if (2.0*gTemp < 1)
//			g = (int)(temp2*255);
//		else //if (3.0*gTemp < 2)
//			g = (int)((temp1+(temp2-temp1)*((2.0/3.0)-gTemp)*6.0)*255);
//
//		//calculate the Blue Component
//		if (6.0*bTemp < 1)
//			b = (int)((temp1+(temp2-temp1)*6.0*bTemp) * 255);
//		else if (2.0*bTemp < 1)
//			b = (int)(temp2*255);
//		else //if (3.0*bTemp < 2)
//			b = (int)((temp1+(temp2-temp1)*((2.0/3.0)-bTemp)*6.0)*255);
//
//        return Color.FromArgb(r,g,b);
		
//	}
	
	public static Color HSLtoColor(float Hue, float Sat, float Lum) 

	{ 

		double r=0,g=0,b=0; 

		double temp1,temp2; 

  

		if(Lum==0) 

		{ 

			r=g=b=0; 

		} 

		else 

		{ 

			if(Sat==0) 

			{ 

				r=g=b=Lum; 

			} 

			else 

			{ 

				temp2 = ((Lum<=0.5) ? Lum*(1.0+Sat) : Lum+Sat-(Lum*Sat)); 

				temp1 = 2.0*Lum-temp2; 

             

				double[] t3=new double[]{Hue+1.0/3.0,Hue,Hue-1.0/3.0}; 

				double[] clr=new double[]{0,0,0}; 

				for(int i=0;i<3;i++) 

				{ 

					if(t3[i]<0) 

						t3[i]+=1.0; 

					if(t3[i]>1) 

						t3[i]-=1.0; 

  

					if(6.0*t3[i] < 1.0) 

						clr[i]=temp1+(temp2-temp1)*t3[i]*6.0; 

					else if(2.0*t3[i] < 1.0) 

						clr[i]=temp2; 

					else if(3.0*t3[i] < 2.0) 

						clr[i]=(temp1+(temp2-temp1)*((2.0/3.0)-t3[i])*6.0); 

					else 

						clr[i]=temp1; 

				} 

				r=clr[0]; 

				g=clr[1]; 

				b=clr[2]; 

			} 

		} 
		return Color.FromArgb((int)(255*r),(int)(255*g),(int)(255*b)); 
	} 



	public static void GetHSL(Color c, out float Hue, out float Sat, out float Lum)
	{
		Hue = c.GetHue()/360f;
		Sat = c.GetSaturation();
		Lum = c.GetBrightness();
	}


											
}
internal class Constants
{
	public static int ITEM_HEIGHT = 18;
	public static int ITEM_PAD = 4;
	public static int ITEM_RIGHT_PAD = 5;
	//  TEXT
	public static int TEXT_HEIGHT = 14;
	public static int TEXT_TOP_PAD = 3;
	public static int TEXT_LEFT_PAD = 30;
	public static int TEXT_RIGHT_PAD = 25;
	public static int TEXT_RIGHT_PAD_NO_ICON = 8;
	//  CHECK BOX
	public static int CHECK_TOP_PAD = 4;
	public static int CHECK_LEFT_PAD = 15;
	public static int CHECK_BOX_SIZE = 12;
	//  EXPANSION BOX
	public static int EXPAND_BOX_TOP_PAD = 5;
	public static int EXPAND_BOX_LEFT_PAD = 3;
	public static int EXPAND_BOX_SIZE = 8;
	//  GROUP
	public static int GRP_INDENT = 3;
	//	LIST ITEMS
	public static int LIST_ITEM_INDENT = 18;
	public static int ICON_RIGHT_PAD = 25;
	public static int ICON_TOP_PAD = 3;
	public static int ICON_SIZE = 13;
    
	//	CONNECTION LINES FROM GROUPS TO SUB ITEMS
	public static int VERT_LINE_INDENT = (GRP_INDENT + 7);
	public static int VERT_LINE_GRP_TOP_OFFSET = 14;
	//	COLOR SCHEME CONSTANTS
	public static int CS_ITEM_HEIGHT = 14;
	public static int CS_TOP_PAD = 1;
	public static int CS_PATCH_WIDTH = 15;
	public static int CS_PATCH_HEIGHT = 12;
	public static int CS_PATCH_LEFT_INDENT = (CHECK_LEFT_PAD);
	public static int CS_TEXT_LEFT_INDENT = (CS_PATCH_LEFT_INDENT + CS_PATCH_WIDTH + 3);
	public static int CS_TEXT_TOP_PAD = 3;
	//	SCROLLBAR
	public static int SCROLL_WIDTH = 15;
	//	MISC
	// DROP_TOLERANCE 4

	public static int INVALID_INDEX = -1;

    // constants for the new symbology
    public static int ICON_WIDTH = 24;
    public static int ICON_HEIGHT = 13;

	//*******************************************************
	//Visual Basic Related constants
	//*******************************************************
	public static int VB_SHIFT_BUTTON = 1;
	public static int VB_LEFT_BUTTON = 1;
	public static int VB_RIGHT_BUTTON = 2;
	

}
