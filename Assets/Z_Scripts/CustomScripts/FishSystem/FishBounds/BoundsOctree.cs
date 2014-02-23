﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Xml.Serialization;

public class BoundsOctree
{
	public float size;
	public Vector3 position;
	public bool Empty = false;
    public bool Obstacle = false;

	public int cornerID;
	public int levelDepth;
	public int Levels;
	
	public List<BoundsOctree> relatedOctrees;
	public bool Related;
	public bool shouldBeDeleted;
	public static List<BoundsOctree> octrees = new List<BoundsOctree>(); //ONLY TO BE USED FOR DEBUG 
	public BoundsOctree[] subOctrees;

	public BoundsOctree parentOctree;
    public List<BoundsOctree> abandonedNeighbors;

	#region properties
	public Vector3 top { get { return position + new Vector3(0, 1, 0)*size; } }
	public Vector3 bottom { get { return position + new Vector3(0, -1, 0) * size; } }
	public Vector3 north { get { return position + new Vector3(0, 0, 1) * size; } }
	public Vector3 south { get { return position + new Vector3(0, 0, -1) * size; } }
	public Vector3 east { get { return position + new Vector3(1, 0, 0) * size; } }
	public Vector3 west { get { return position + new Vector3(-1, 0, 0) * size; } }

	public Vector3[] sides;


	public static readonly short[,] AdjacentDirections = new short[26,3]
	{
		{1,0,0},
		{-1,0,0},

		{1,1,0},
		{-1,-1,0},
		{-1,1,0},
		{1,-1,0},

		{1,0,1},
		{-1,0,-1},
		{1,0,-1},
		{-1,0,1},

		{1,1,1},
		{-1,1,1},
		{-1,-1,1},
		{-1,-1,-1},
		{1,-1,-1},
		{1,1,-1},
		{1,-1,1},
		{-1,1,-1},
		
		{0,1,1},
		{0,-1,-1},
		{0,-1,1},
		{0,1,-1},

		{0,0,1},
		{0,0,-1},

		{0,1,0},
		{0,-1,0}

		

	};

    public Vector3[] cornerCentre;
    public Vector3[] corners;

	public List<BoundsOctree> neighbors = new List<BoundsOctree>();

	public int binaryLocation_X;
	public int binaryLocation_Y;
	public int binaryLocation_Z;
	#endregion
	public BoundsOctree(BoundsOctree parent, int _cornerID, string _name, float s, Vector3 pos, int maxL, int maxPop,List<BoundsOctree> relatedOcs)
	{
		//Debug.Log("Creating an BoundsOctree!");
		//octrees.Add(this);
		if (relatedOcs == null)
		{
			relatedOctrees = new List<BoundsOctree>();
			relatedOctrees.Add(this);
		}
		else
		{
			relatedOctrees = relatedOcs;
			relatedOctrees.Add(this);
		}
		parentOctree = parent;
		cornerID = _cornerID;
		//name = _name;
		levelDepth = maxL;
		size = s;
		if (parentOctree != null)
		{
            position = parentOctree.cornerCentre[cornerID];
			//octreeID = parentOctree.octreeID*10 + cornerID;
			//binaryLocation_X 
			binaryLocation_X = parentOctree.binaryLocation_X;
			binaryLocation_Y = parentOctree.binaryLocation_Y;
			binaryLocation_Z = parentOctree.binaryLocation_Z;
			if (cornerID % 2 != 0)
			{
				binaryLocation_X += (int) Math.Pow(2, levelDepth);
			}
			if (cornerID > 3)
			{
				binaryLocation_Y += (int)Math.Pow(2, levelDepth);
				
			}
			if (cornerID == 2 || cornerID == 3 || cornerID == 6 || cornerID == 7)
			{
				binaryLocation_Z += (int)Math.Pow(2, levelDepth);
			}
			Levels = parentOctree.Levels;
		}
		else
		{
			Levels = levelDepth;
		   // octreeID = 1;
			position = pos;
			binaryLocation_X += (int)Math.Pow(2, levelDepth);
			binaryLocation_Y += (int)Math.Pow(2, levelDepth);
			binaryLocation_Z += (int)Math.Pow(2, levelDepth);
		}
	   // maxPopulation = maxPop;
		sides = new Vector3[6];
		sides[0] = top;
		sides[1] = bottom;
		sides[2] = north;
		sides[3] = south;
		sides[4] = east;
		sides[5] = west;



        cornerCentre = new Vector3[8];
        cornerCentre[0] = new Vector3(-0.5f,-0.5f,-0.5f)*size+position;
        cornerCentre[1] = new Vector3(0.5f, -0.5f, -0.5f) * size + position; 
        cornerCentre[2] = new Vector3(-0.5f,-0.5f,0.5f) * size + position;
        cornerCentre[3] = new Vector3(0.5f, -0.5f, 0.5f) * size + position;
        cornerCentre[4] = new Vector3(-0.5f, 0.5f, -0.5f) * size + position;
        cornerCentre[5] = new Vector3(0.5f, 0.5f, -0.5f) * size + position;
        cornerCentre[6] = new Vector3(-0.5f, 0.5f, 0.5f) * size + position;
        cornerCentre[7] = new Vector3(0.5f, 0.5f, 0.5f) * size + position;

        corners = new Vector3[8];
        corners[0] = new Vector3(-1f, -1f, -1f) * size + position;
        corners[1] = new Vector3(1f, -1f, -1f) * size + position;
        corners[2] = new Vector3(-1f, -1f, 1f) * size + position;
        corners[3] = new Vector3(1f, -1f, 1f) * size + position;
        corners[4] = new Vector3(-1f, 1f, -1f) * size + position;
        corners[5] = new Vector3(1f, 1f, -1f) * size + position;
        corners[6] = new Vector3(-1f, 1f, 1f) * size + position;
        corners[7] = new Vector3(1f, 1f, 1f) * size + position;
		
	}
	public void ClearOctree()
	{
        if (parentOctree.abandonedNeighbors == null)
        {
            parentOctree.abandonedNeighbors = new List<BoundsOctree>();
        }
        else
        {
            if (parentOctree.abandonedNeighbors.Contains(this))
            {
                parentOctree.abandonedNeighbors.Remove(this);
            }
        }

        
        foreach (var neighbor in neighbors)
        {
            if(!parentOctree.abandonedNeighbors.Contains(neighbor))
            {
                parentOctree.abandonedNeighbors.Add(neighbor);
            }
        }
		//relatedOctrees.Remove(this);
		//octrees.Remove(this);
		/*
		for (int index = neighbors.Count - 1; index > 0; index--)
		{
			if (neighbors[index] != null)
			{
				neighbors[index].neighbors.Remove(this);
				neighbors[index].AddNeighbor(parentOctree);
			}
		}*/
		shouldBeDeleted = true;
		//neighbors.Clear();
		//Debug.Log("Prutt");
	}
	public void RefreshNeighbors()
	{
		if (neighbors == null)
		{
			neighbors = new List<BoundsOctree>();
		}
		else
		{
			neighbors.Clear();
		}
		if (subOctrees != null || !Empty)
		{
			return;
		}
		if (parentOctree != null)
		{
			for (int i = 0; i < 26; i++)
			{                
				LocateNeighbor(this,AdjacentDirections[i, 0], AdjacentDirections[i,1], AdjacentDirections[i,2]);

			}
		}
        if (abandonedNeighbors != null)
        {
            foreach (var abandonedNeighbor in abandonedNeighbors)
            {
                abandonedNeighbor.RefreshNeighbors();
            }
            abandonedNeighbors = null;
        }
	}
	public void getAdjacent(int workingID, int targetID, BoundsOctree target, List<BoundsOctree> neighBorList)
	{
		for (int i = 0; i < 8; i++)
		{
		//    if (i != target.cornerID)
			{
				//subOctrees[i]
			}
		}
	}

	private BoundsOctree _commonAncestor;
	private BoundsOctree _rightNeighbor;
	public void AddNeighbor(BoundsOctree neighbor)
	{
		if (neighbor == null|| neighbor == this)
		{
			return;
		}
		if (neighbors == null)
		{
			neighbors = new List<BoundsOctree>();
		}
		if (!neighbors.Contains(neighbor))
		{
			neighbors.Add(neighbor);
		}
	}
	public BoundsOctree LocateNeighbor(BoundsOctree source, short _x, short _y, short _z)
	{
		int binaryCellSize = 1 << levelDepth;

		int xLoc = binaryLocation_X;
		int yLoc = binaryLocation_Y;
		int zLoc = binaryLocation_Z;

		if ((binaryLocation_X + binaryCellSize)*_x < (1 << (Levels + 1))&&
			(binaryLocation_X + (1 << Levels)*_x)>0)
		{
			if(_x == -1)
			{
				xLoc = binaryLocation_X - 0x00000001;
			}
			else
			{
				xLoc += binaryCellSize*_x;
			}

			if (
				(binaryLocation_Y + binaryCellSize) * _y < (1 << (Levels + 1)) &&
				(binaryLocation_Y + (1 << Levels) * _y) > 0)
			{
				if (_y == -1)
				{
					yLoc = binaryLocation_Y - 0x00000001;
				}
				else
				{
					yLoc += binaryCellSize*_y;
				}

				if (
					(binaryLocation_Z + binaryCellSize)*_z < (1 << (Levels + 1)) &&
					(binaryLocation_Z + (1 << Levels)*_z) > 0)
				{
					if (_z == -1)
					{
						zLoc = binaryLocation_Z - 0x00000001;
					}
					else
					{
						zLoc += binaryCellSize*_z;
					}
					GetCommonAncestor(this, binaryLocation_X ^ xLoc, binaryLocation_Y ^ yLoc, binaryLocation_Z ^ zLoc).
					AddToNeighbors(source,xLoc, yLoc, zLoc);
				}
			}
		}
		return null;
	}
	public BoundsOctree GetCommonAncestor(BoundsOctree cell, int xDiff, int yDiff, int zDiff)
	{
		BoundsOctree c = cell;

		while (c.parentOctree != null && ((xDiff & (1 << c.levelDepth))>0
			||  (yDiff & (1 << c.levelDepth))>0
			||  (zDiff & (1 << c.levelDepth))>0))
		{
			c = c.parentOctree;
		}
		return c;
	}
	public BoundsOctree TraverseToOctree( int targetXLoc, int targetYLoc, int targetZLoc)
	{
		
		if (subOctrees == null)
		{
			return this;
		}
		else
		{
			int childBranchBit = 1 << (levelDepth - 1);
			int childIndex;

			int binaryCellSize = 1 << levelDepth;

			childIndex =
			(((targetXLoc & childBranchBit) >> levelDepth - 1)) +
			(((targetYLoc & childBranchBit) >> levelDepth - 1) * 4) +
			(((targetZLoc & childBranchBit) >> levelDepth - 1) * 2);
			return subOctrees[childIndex].TraverseToOctree(targetXLoc, targetYLoc, targetZLoc);
		}
		   
	}
	public void AddToNeighbors(BoundsOctree source, int targetXLoc, int targetYLoc, int targetZLoc)
	{
		if (source == null)
		{
			return;
		}
		if (subOctrees == null && Empty)
		{
			source.AddNeighbor(this);
		}
		else
		{
			int childBranchBit = 1 << (levelDepth - 1);
			int childIndex;
			if (source.levelDepth >= levelDepth &&subOctrees!=null)
			{
				int binaryCellSize = 1 << levelDepth;
				bool x = (targetXLoc < source.binaryLocation_X || targetXLoc >= source.binaryLocation_X + binaryCellSize);
				bool y = (targetYLoc < source.binaryLocation_Y || targetYLoc >= source.binaryLocation_Y + binaryCellSize);
				bool z = (targetZLoc < source.binaryLocation_Z || targetZLoc >= source.binaryLocation_Z + binaryCellSize);

				
				if(x&&y&&z)
				{
					childIndex = 
					(((targetXLoc & childBranchBit) >> levelDepth - 1)) +
					(((targetYLoc & childBranchBit) >> levelDepth - 1) * 4) +
					(((targetZLoc & childBranchBit) >> levelDepth - 1) * 2);
					subOctrees[childIndex].AddToNeighbors(source, targetXLoc, targetYLoc, targetZLoc);
				}
				else if (!x && !z)
				{

					int offset = (((targetYLoc & childBranchBit) >> levelDepth - 1)*4);
					subOctrees[0 + offset].AddToNeighbors(source, targetXLoc, targetYLoc, targetZLoc);
					subOctrees[1 + offset].AddToNeighbors(source, targetXLoc, targetYLoc, targetZLoc);
					subOctrees[2 + offset].AddToNeighbors(source, targetXLoc, targetYLoc, targetZLoc);
					subOctrees[3 + offset].AddToNeighbors(source, targetXLoc, targetYLoc, targetZLoc);

				}
				else if(!x && !y) 
				{
					int offset = (((targetZLoc & childBranchBit) >> levelDepth - 1) * 2);
					subOctrees[0 + offset].AddToNeighbors(source, targetXLoc, targetYLoc, targetZLoc);
					subOctrees[1 + offset].AddToNeighbors(source, targetXLoc, targetYLoc, targetZLoc);
					subOctrees[4 + offset].AddToNeighbors(source, targetXLoc, targetYLoc, targetZLoc);
					subOctrees[5 + offset].AddToNeighbors(source, targetXLoc, targetYLoc, targetZLoc);
				}
				else if (!y && !z)
				{
					int offset = (((targetXLoc & childBranchBit) >> levelDepth - 1) );
					subOctrees[0 + offset].AddToNeighbors(source, targetXLoc, targetYLoc, targetZLoc);
					subOctrees[2 + offset].AddToNeighbors(source, targetXLoc, targetYLoc, targetZLoc);
					subOctrees[4 + offset].AddToNeighbors(source, targetXLoc, targetYLoc, targetZLoc);
					subOctrees[6 + offset].AddToNeighbors(source, targetXLoc, targetYLoc, targetZLoc);
				}
				else if(!x)
				{
					int offset = (((targetYLoc & childBranchBit) >> levelDepth - 1) * 4) +
						 (((targetZLoc & childBranchBit) >> levelDepth - 1) * 2);
					if (subOctrees != null)
					{
						subOctrees[0 + offset].AddToNeighbors(source, targetXLoc, targetYLoc, targetZLoc);
						subOctrees[1 + offset].AddToNeighbors(source, targetXLoc, targetYLoc, targetZLoc); 
					}
					else
					{
						Debug.Log("SHUIT!");
					}
				}
				else if(!y)
				{
					int offset = (((targetXLoc & childBranchBit) >> levelDepth - 1)) +
						 (((targetZLoc & childBranchBit) >> levelDepth - 1) * 2);
					subOctrees[0 + offset].AddToNeighbors(source, targetXLoc, targetYLoc, targetZLoc);
					subOctrees[4 + offset].AddToNeighbors(source, targetXLoc, targetYLoc, targetZLoc);
				}
				else //z
				{
					int offset = (((targetXLoc & childBranchBit) >> levelDepth - 1)) +
								 (((targetYLoc & childBranchBit) >> levelDepth - 1)*4);
					subOctrees[0 + offset].AddToNeighbors(source, targetXLoc, targetYLoc, targetZLoc);
					subOctrees[2 + offset].AddToNeighbors(source, targetXLoc, targetYLoc, targetZLoc);
				}
				

			}
			else
			{

				if (subOctrees != null)
				{
					childIndex =
					(((targetXLoc & childBranchBit) >> levelDepth - 1)) +
					(((targetYLoc & childBranchBit) >> levelDepth - 1) * 4) +
					(((targetZLoc & childBranchBit) >> levelDepth - 1) * 2);
					subOctrees[childIndex].AddToNeighbors(source, targetXLoc, targetYLoc, targetZLoc);

				}
				
			}
			

		}
	}
	/*public BoundsOctree TraverseToOctree(int targetXLoc,int targetYLoc,int targetZLoc)
	{
		if(subOctrees == null || (binaryLocation_X == targetXLoc && binaryLocation_Y == targetYLoc &&
			binaryLocation_Z == targetZLoc))
		{
			return this;
		}
		else
		{

			int childBranchBit = 1 << (levelDepth - 1);

			int childIndex = 
				(((targetXLoc & childBranchBit) >> levelDepth - 1)) +
				(((targetYLoc & childBranchBit) >> levelDepth - 1)*4) +
				(((targetZLoc & childBranchBit) >> levelDepth - 1)*2) ;


			return subOctrees[childIndex].TraverseToOctree(targetXLoc, targetYLoc, targetZLoc);
		  
		}
	}
	*/
	
    public void GL_Draw(bool DrawObstacle,bool DrawEmpty,bool DrawNonEmpty)
	{
        

        if (subOctrees != null)
        {
            foreach (var subOctree in subOctrees)
            {
                subOctree.GL_Draw(DrawObstacle,DrawEmpty,DrawNonEmpty);
            }
        }
        else
        {
            
            if (Empty)
            {
                GL.Color(Color.yellow);
            }
            else
            {
               
                GL.Color(Color.red);

            }
            if (Obstacle)
            {
                GL.Color(Color.blue);

            }
            if ((Obstacle&& DrawObstacle) || (Empty&&DrawEmpty) || (!Empty && DrawNonEmpty))
            {
                GL.Vertex(corners[0]);
                GL.Vertex(corners[1]);
                GL.Vertex(corners[0]);
                GL.Vertex(corners[2]);
                GL.Vertex(corners[1]);
                GL.Vertex(corners[3]);
                GL.Vertex(corners[2]);
                GL.Vertex(corners[3]);

                GL.Vertex(corners[4]);
                GL.Vertex(corners[5]);
                GL.Vertex(corners[4]);
                GL.Vertex(corners[6]);
                GL.Vertex(corners[5]);
                GL.Vertex(corners[7]);
                GL.Vertex(corners[6]);
                GL.Vertex(corners[7]);

                GL.Vertex(corners[0]);
                GL.Vertex(corners[4]);
                GL.Vertex(corners[1]);
                GL.Vertex(corners[5]);
                GL.Vertex(corners[2]);
                GL.Vertex(corners[6]);
                GL.Vertex(corners[3]);
                GL.Vertex(corners[7]);
            }
           
        }
	}
	public void DrawEmptyGizmos()
	{
		if (subOctrees != null)
		{
			foreach (var subOctree in subOctrees)
			{
				subOctree.DrawEmptyGizmos();
			}
		}
		else
		{
			if (Empty)
			{
				if (Related)
				{
					Gizmos.color = Color.green;

				}
				else
				{
					Gizmos.color = Color.yellow;

				}
				Gizmos.DrawWireCube(position, (Vector3.one * size * 2) - Vector3.one * 0.01f);


			}
		}

	}
	public void DrawFullGizmos()
	{
		if (subOctrees != null)
		{
			foreach (var subOctree in subOctrees)
			{
				subOctree.DrawFullGizmos();
			}
		}
		else
		{
			if (!Empty)
			{
				if (Related)
				{
					Gizmos.color = Color.magenta;

				}
				else
				{
					Gizmos.color = Color.red;

				}
				Gizmos.DrawWireCube(position, (Vector3.one * size * 2) - Vector3.one * 0.01f);


			}
		}

	}
	public void DrawGizmos()
	{
		if (subOctrees != null)
		{
			foreach (var subOctree in subOctrees)
			{
				subOctree.DrawGizmos();
			}
		}
		else
		{
			if (Empty)
			{
				if (Related)
				{
					Gizmos.color = Color.green;
					//Gizmos.DrawWireCube(position, (Vector3.one * size * 2) - Vector3.one * 0.05f);
					
				}
				else
				{
					Gizmos.color = Color.yellow;
					
				}

		
			}
			else
			{
				if (Related)
				{
					Gizmos.color = Color.magenta;
					

				}
				else
				{
					Gizmos.color = Color.red;

				}
				Gizmos.DrawWireCube(position, (Vector3.one * size * 2) - Vector3.one * 0.05f);
				
			}
			if (shouldBeDeleted)
			{
				Gizmos.color = Color.blue;
			}
			//Gizmos.DrawWireCube(position, (Vector3.one * size * 2)-Vector3.one*0.05f);

			//Gizmos.DrawWireCube(position, Vector3.one * size*2);
		}

	}
	public void drawLocalGizmos()
	{
		Gizmos.color = Color.green;

		Gizmos.DrawWireCube(position, Vector3.one * size * 1.99f);
		if (neighbors.Count >= 1)
		{
			Color col = Gizmos.color;
			col.a = 0.95f;
			Gizmos.color = col;

			Gizmos.DrawCube(position, Vector3.one * size * 1.9f);
		}

		if (_commonAncestor != null)
		{
			Gizmos.color = Color.blue;

			Gizmos.DrawWireCube(_commonAncestor.position, Vector3.one * _commonAncestor.size * 1.99f);
		}
		
		foreach (var neighbor in neighbors)
		{
			Gizmos.color = Color.yellow;

			Gizmos.DrawWireCube(neighbor.position, Vector3.one * neighbor.size * 1.9f);
			Color col = Gizmos.color;
			col.a = 0.1f;
			Gizmos.color = col;

			Gizmos.DrawCube(neighbor.position, Vector3.one * neighbor.size * 1.9f);

		}
		
	}

	public void EstablishRelations()
	{
		Related = true;
		foreach (var boundsOctree in neighbors)
		{
			if(!boundsOctree.Related)
			{
				boundsOctree.EstablishRelations();
			}
		}
	}
	public bool CleanOutUnrelated()
	{
		if (subOctrees != null)
		{
			if (Related)
			{
				return false;
			}

			bool gotRelatedChildren = false;
			foreach (var boundsOctree in subOctrees)
			{
				if (!boundsOctree.CleanOutUnrelated())
				{
					gotRelatedChildren = true;
				}
			}
			if (gotRelatedChildren)
			{
				return false;
			}
			for (int index = subOctrees.Length-1; index >0; index--)
			{
				
				subOctrees[index].ClearOctree();
				subOctrees[index] = null;
			}
			subOctrees = null;
		}
		else if(Related)
		{
			return false;
		}
		return true;

	}

	public void CheckBounds(LayerMask obstacleMask)
	{
		if (Physics.CheckSphere(position, size*1.732f,obstacleMask) )
		{
			if (levelDepth >= 1)
			{
				subOctrees = new BoundsOctree[8];
				bool subsEmpty = true;
				for (int i = 0; i < 8; i++)
				{
					subOctrees[i] = new BoundsOctree(this, i, "Name", size * 0.5f,
						position, levelDepth - 1,999,relatedOctrees);
					subOctrees[i].CheckBounds(obstacleMask);
					if (!subOctrees[i].Empty)
					{
						subsEmpty = false;
					}
				}
				if (subsEmpty)
				{
					for (int i = 0; i < 8; i++)
					{
						subOctrees[i] = null;
					}
					subOctrees = null;
					Empty = true;
				}
			}
		}
		else
		{
			Empty = true;
		}
	}

	public BoundsOctree GetBound(Vector3 point)
	{
		if (isPointInside(point))
		{
			if (subOctrees != null)
			{
				
				foreach (var subOctree in subOctrees)
				{
					BoundsOctree bo = subOctree.GetBound(point);
					if (bo != null)
					{
						return bo;
					}
				}
			}
			else
			{
				if (Empty)
				{
					return this;
				}
			}
		}
		return null;
	}
	public BoundsOctree GetBound(bool b,Vector3 point)
	{
		if (isPointInside(point))
		{
			if (subOctrees != null)
			{
				
				foreach (var subOctree in subOctrees)
				{
					BoundsOctree bo = subOctree.GetBound(point);
					if (bo != null)
					{
						return bo;
					}
				}
			}
			else
			{
				
				 return this;
				
			}
		}
		return null;
	}

    public void GetBounds(List<BoundsOctree> foundBounds,Vector3 point,float radius)
    {
        if(isSphereInside(point,radius))
        {
            if (subOctrees != null)
            {
                foreach (var subOctree in subOctrees)
                {
                    subOctree.GetBounds(foundBounds,point, radius);

                }
            }
            else
            {
                if (levelDepth >= 1)
                {
                    subOctrees = new BoundsOctree[8];
                    bool AllSubsObstacled = true;
                    for (int i = 0; i < 8; i++)
                    {
                        subOctrees[i] = new BoundsOctree(this, i, "Name", size * 0.5f,
                            position, levelDepth - 1, 999, relatedOctrees);
                        subOctrees[i].Empty = Empty;
                        subOctrees[i].GetBounds(foundBounds, point, radius);
                        if (!subOctrees[i].Obstacle)
                        {
                            AllSubsObstacled = false;
                        }
                    }
                    if (AllSubsObstacled)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            subOctrees[i].ClearOctree(); 
                            subOctrees[i] = null;
                        }
                        subOctrees = null;
                        //RefreshNeighbors();


                    }
                }
                else if(Empty)
                {
                    if (foundBounds == null)
                    {
                        foundBounds = new List<BoundsOctree>();
                    }
                    if (!foundBounds.Contains(this))
                    {
                        foundBounds.Add(this);
                        Obstacle = true;
                    }
                }

                
            }
               

        }

    }

    public void CleanupObstacleGhost()
    {
        if (subOctrees != null)
        {
            bool AllSubsObstacled = true;
            for (int i = 0; i < 8; i++)
            {
                if (subOctrees[i].Obstacle || !subOctrees[i].Empty || subOctrees[i].subOctrees!=null)
                {
                    AllSubsObstacled = false;
                }
            }
            if (AllSubsObstacled)
            {
                for (int i = 0; i < 8; i++)
                {
                    subOctrees[i].ClearOctree(); 

                    subOctrees[i] = null;
                }
                subOctrees = null;
                //RefreshNeighbors();
            }
        }
        if (subOctrees == null)
        {
            if (parentOctree != null)
            {
                parentOctree.CleanupObstacleGhost();
            }
        }

    }


	public BoundsOctree GetBoundByBinary(Vector3 point)
	{
		point = (point - position);
		//int b_x = (int)(point.x+size);
		byte b_x = ConvertCartesianToBinary(point.x);
		byte b_y = ConvertCartesianToBinary(point.y);
		byte b_z = ConvertCartesianToBinary(point.z);

		return TraverseToOctree(b_x,b_y,b_z);
	}
	public byte ConvertCartesianToBinary(float p)
	{
		int point = ((int)(((p + size) / (size * 2.0f)) * 256)-1);
   /*     int b_p = 0;
		int t = 0;
		for(int i = levelDepth;i>0; i--)
		{
			t = (int) Math.Pow(2,i)
			if(point >= t)
			{
				point -= t;
				b_p += t;
			}
	 */   
		return (byte)point;
		//return ((byte)(((p + size) / (size * 2.0f)) * 256));
	}

	public bool isPointInside(Vector3 point)
	{
		if (point.y <= top.y && point.y > bottom.y && point.x <= east.x &&
			point.x > west.x && point.z <= north.z && point.z > south.z)
		{
			return true;
		}
		return false;
	}
    public bool isSphereInside(Vector3 point,float size) // i did think of diagonal lines, need fix.
    {
        if (point.y <= top.y+size && point.y > bottom.y-size 
            && point.x <= east.x+size && point.x > west.x-size 
            && point.z <= north.z+size && point.z > south.z-size)
        {
            return true;
        }
        return false;
    }


	public void SaveOctreeToStream(BinaryWriter bw)
	{
		bw.Write(Empty);
		bw.Write(Related);
		if (subOctrees != null)
		{
			bw.Write(true); //HasChildren
			foreach (var boundsOctree in subOctrees)
			{
				boundsOctree.SaveOctreeToStream(bw);
			}
		}
		else
		{
			bw.Write(false);
		}
		
	}

	public void CreateFromStream(BinaryReader br)
	{
		Empty = br.ReadBoolean();
		Related = br.ReadBoolean();
		if (br.ReadBoolean())
		{
			subOctrees = new BoundsOctree[8];

			for (int i = 0; i < 8; i++)
			{
				subOctrees[i] = new BoundsOctree(this, i, "Name", size * 0.5f, position, levelDepth - 1, 999, relatedOctrees);
				subOctrees[i].CreateFromStream(br);
				//subOctrees[i].CheckBounds(obstacleMask);
			}
		}

	}
}