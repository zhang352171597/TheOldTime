using UnityEngine;
using System.Collections;

public delegate void VoidDelegateInt(int i);
public delegate void VoidDelegateIntInt(int i , int ii);
public delegate void VoidDelegateMessage(params System.Object[] objs);
public delegate bool BoolDelegateMessage(params System.Object[] objs);
public delegate int IntDelegateMessage(params System.Object[] objs);
public delegate float FloatDelegateMessage(params System.Object[] objs);
public delegate System.Object[] ObjectsDelegateMessage(params System.Object[] objs);

