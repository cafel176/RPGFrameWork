using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class gameUI : MonoBehaviour
    {
        private gameUIData data = new gameUIData();

        [SerializeField]
        private string dayKey;
        [SerializeField]
        private GameObject[] days;

        [SerializeField]
        private string[] itemKeys;
        [SerializeField]
        private GameObject[] items;

        [SerializeField]
        private string frgKey;
        [SerializeField]
        private GameObject[] frgs;


        private void Update()
        {
            int num = data.getInt(dayKey);
            for(int i=0;i< days.Length; i++)
            {
                if(i<num)
                    days[i].SetActive(true);
                else
                    days[i].SetActive(false);
            }

            num = data.getInt(frgKey);
            for (int i = 0; i < frgs.Length; i++)
            {
                if (i < num)
                    frgs[i].SetActive(true);
                else
                    frgs[i].SetActive(false);
            }

            for (int i = 0; i < items.Length; i++)
            {
                if (data.getSwitch(itemKeys[i]))
                    items[i].SetActive(true);
                else
                    items[i].SetActive(false);
            }
        }

        public void useItem(int i)
        {
            data.setSwitch(itemKeys[i], false);
        }
    }
}

