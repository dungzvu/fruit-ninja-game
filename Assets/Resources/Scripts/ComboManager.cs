using UnityEngine;
using System.Collections;

public class ComboManager
{
    //luu gia tri n la so fruit chat dc
    private int nFruits=0;
    private Vector3 position;
    //neu qua nDelayFrame ma khong nhan duoc fruit thi tinh combo
    private int nDelayFrame=5;
    private int minFruitsForCombo=3;

    private int count = 0;

    public void addFruit(int n,Vector3 pos)
    {
        nFruits += n;
        position = pos;
        count = 0;
    }


    public bool Update(out int n, out Vector3 pos)
    {
        bool result = false;
        n = 0;
        pos = Vector3.zero;
        if(count<nDelayFrame){
            count++;
        }
        else if(count==nDelayFrame)
        {
            
            //tinh combo
            if (nFruits >= minFruitsForCombo)
            {
                n = nFruits;
                pos = position;
                result = true;
            }
            nFruits = 0;
            count = nDelayFrame + 1;
        }
        return result;
    }


	
}
