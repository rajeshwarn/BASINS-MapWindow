// Extent.h: interface for the Extent class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_EXTENT_H__CCC7B001_1ECF_11D5_A566_00104BCC583E__INCLUDED_)
#define AFX_EXTENT_H__CCC7B001_1ECF_11D5_A566_00104BCC583E__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "stdafx.h"

class Extent  
{
public:
	/*Extent();
	virtual ~Extent();
	Extent( double Left, double Right, double Bottom, double Top );*/

	Extent::Extent()
	{	left = 0;
		right = 0;
		bottom = 0;
		top = 0;
	}

	Extent::~Extent()
	{

	}

	Extent::Extent( double Left, double Right, double Bottom, double Top )
	{	
		left = Left;
		right = Right;
		bottom = Bottom;
		top = Top;
	}

	double left;
	double right;
	double bottom;
	double top;

	Extent& Extent::operator=(const Extent& ext)
	{
		if (&ext == this)
			return *this;

		this->bottom = ext.bottom;
		this->left = ext.left;
		this->right = ext.right;
		this->top = ext.top;

		return *this;
	}

	bool Extent::operator==(const Extent& ext)
	{
		if (this->bottom != ext.bottom) return false;
		if (this->left != ext.left) return false;
		if (this->right != ext.right) return false;
		if (this->top != ext.top) return false;
		return true;
	}
};

#endif // !defined(AFX_EXTENT_H__CCC7B001_1ECF_11D5_A566_00104BCC583E__INCLUDED_)
