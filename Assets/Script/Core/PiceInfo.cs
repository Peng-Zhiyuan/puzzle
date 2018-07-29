using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PiceInfo
{
    public int index;
    public int boardX;
    public int boardY;
    public int sideIndex;
    public bool isFixed;
    public PiceOwner owner;
    public List<LinkingInfo> LinkingInfoList = new List<LinkingInfo>();
    public EdgeType leftType;
    public EdgeType rightType;
    public EdgeType bottomType;
    public EdgeType topType;
    public int sortingOrder;

}

public class LinkingInfo
{
    public int piceIndex;
    public LinkDirectory directory;
}

public class PuzzleInfo
{
    public List<PiceInfo> piceInfoList = new List<PiceInfo>();

}