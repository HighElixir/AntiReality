﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace HE_AntiReality
{
    public class AddBodyParts
    {    //１に義体を追加する場所、２で義体を指定して追加できる.
         //もし同じdefNameのパーツが２か所以上存在する場合、
         //3にuntranslatedCustomLabelを記入すれば全ての場所に追加できる。
         //ほかModへの適応性を考慮して、なるべく簡単な英単語を入れる

        public BodyPartDef createHediffOn;//1

        public HediffDef createHediff;//2

        public string partsLabel;//3
    }
    public class HediffCompProperties_MakeBodyParts : HediffCompProperties
    {
        public List<AddBodyParts> addBodyParts;

        public HediffCompProperties_MakeBodyParts()
        {
            compClass = typeof(HediffComp_MakeBodyParts);
        }
    }
}

