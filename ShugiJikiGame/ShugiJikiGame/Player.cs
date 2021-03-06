﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShugiJikiGame
{
    class Player
    {
        /// <summary>
        /// ニンジャスレイヤーならばTrue
        /// </summary>
        public bool IsNjslyr { get; set; }

        /// <summary>
        /// 周回数
        /// </summary>
        public int LapCount { get; set; }

        /// <summary>
        /// アンブッシュ成功回数
        /// </summary>
        public int AmbushCount { get; set; }

        /// <summary>
        /// 外周の現在値
        /// </summary>
        public int PositionOuter { get; set; }
        /// <summary>
        /// シュギ・ジキの間での現在値
        /// </summary>
        public int PositionShugi { get; set; }
        /// <summary>
        /// シュギ・ジキの間での開始位置
        /// </summary>
        public int PositionShugiStart { get; set; }
        /// <summary>
        /// シュギ・ジキ内部にいるかどうか
        /// </summary>
        public bool IsShugi { get; set; }
        /// <summary>
        /// アンブッシュされて休む回数
        /// </summary>
        public int Damage { get; set; }
        /// <summary>
        /// 最大ライフ／最大蓄積ダメージ
        /// </summary>
        public int MaxLife { get; set; }
        /// <summary>
        /// 死亡フラグ
        /// </summary>
        public bool IsDead { get; set; }

        public Player(bool isNinjaSlayer)
        {
            this.IsNjslyr = isNinjaSlayer;
            this.LapCount = 0;
            this.AmbushCount = 0;
            this.PositionOuter = this.IsNjslyr ? 10 : 0;
            this.PositionShugi = 0;
            this.IsShugi = false;
            this.Damage = 0;
            this.IsDead = false;
            this.MaxLife = 2;
        }
    }
}
