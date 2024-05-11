using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseScript
{

    public  void StartFunction();
    public  void ReStartFunction();
    public  void OnDestroyFunction();
    public  void UpdateFunction();


    public virtual void FixedUpdateFunction()
    {

    }
    public virtual void LateUpdateFunction()
    {

    }

    //public void StartFunction()
    //{
    //}
    //public void OnDestroyFunction()
    //{
    //}

    //public void UpdateFunction()
    //{
    //}



    //public void ReStartFunction()
    //{
    //}

}
