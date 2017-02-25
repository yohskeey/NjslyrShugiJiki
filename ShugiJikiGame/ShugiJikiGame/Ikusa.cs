using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShugiJikiGame
{
    class Ikusa
    {
        public Player MyPlayer { get; set; }
        public Player NinjaSlayer { get; set; }

        public Ikusa(bool isNSMaxLifeTwo)
        {
            MyPlayer = new Player(false);
            NinjaSlayer = new Player(true);
            NinjaSlayer.MaxLife = isNSMaxLifeTwo ? 2 : int.MaxValue;
        }

        public static int StartDice()
        {
            var now = int.Parse(DateTime.Now.ToString("MMddHHmmss")) + DateTime.Now.Millisecond;
            var rand = new Random(now);

            return rand.Next() % 6 + 1;
        }

        public List<string> MovePlayer(int Dice)
        {
            var msg = new List<string>();
            var next = 0;
            if (MyPlayer.IsShugi)
            {
                // 1マスごとにアンブッシュ判定
                for (int i = 0; i < Dice; i++)
                {
                    // 次のマスが4ならば0とする
                    next = (MyPlayer.PositionShugi + 1 == 4) ? 0 : MyPlayer.PositionShugi + 1 ;
                    if (NinjaSlayer.IsShugi && next == NinjaSlayer.PositionShugi)
                    {
                        // 赤黒に追い付いた
                        NinjaSlayer.Damage = (NinjaSlayer.Damage + 2 > NinjaSlayer.MaxLife) ? NinjaSlayer.MaxLife : NinjaSlayer.Damage + 2;
                        MyPlayer.AmbushCount++;
                        msg.Add("「イヤーッ！」「グワーッ！」ニンジャスレイヤーにアンブッシュ成功！");
                    }
                    MyPlayer.PositionShugi = next;
                }
                
                // 開始地点ならばシュギ・ジキから出る
                if(MyPlayer.PositionShugi == MyPlayer.PositionShugiStart)
                {
                    MyPlayer.IsShugi = false;
                    msg.Add("あなたはシュギ・ジキから離脱した");

                    if (NinjaSlayer.IsShugi == false && MyPlayer.PositionOuter == NinjaSlayer.PositionOuter)
                    {
                        // 赤黒に追い付いた
                        NinjaSlayer.Damage = (NinjaSlayer.Damage + 2 > NinjaSlayer.MaxLife) ? NinjaSlayer.MaxLife : NinjaSlayer.Damage + 2;
                        MyPlayer.AmbushCount++;
                        msg.Add("「イヤーッ！」「グワーッ！」ニンジャスレイヤーにアンブッシュ成功！");
                    }
                }
            }
            else
            {
                // 1マスごとにアンブッシュ判定
                for (int i = 0; i < Dice; i++)
                {
                    // 次のマスが20ならば0とする
                    next = MyPlayer.PositionOuter + 1;
                    if(next == 20)
                    {
                        next = 0;
                        MyPlayer.LapCount++;
                        msg.Add(MyPlayer.LapCount + 1 + "週目突入！");
                    }
                    if (NinjaSlayer.IsShugi == false && next == NinjaSlayer.PositionOuter)
                    {
                        // 赤黒に追い付いた
                        NinjaSlayer.Damage += 2;
                        MyPlayer.AmbushCount++;
                        msg.Add("「イヤーッ！」「グワーッ！」ニンジャスレイヤーにアンブッシュ成功！");
                    }
                    MyPlayer.PositionOuter = next;
                }

                // 開始地点ならばシュギ・ジキに入る
                if (MyPlayer.PositionOuter % 5 == 0)
                {
                    MyPlayer.IsShugi = true;
                    MyPlayer.PositionShugiStart = MyPlayer.PositionOuter / 5;
                    MyPlayer.PositionShugi = MyPlayer.PositionShugiStart;
                    msg.Add("あなたはシュギ・ジキの部屋に入った");

                    if (NinjaSlayer.IsShugi && MyPlayer.PositionShugi == NinjaSlayer.PositionShugi)
                    {
                        // 赤黒に追い付いた
                        NinjaSlayer.Damage += 2;
                        MyPlayer.AmbushCount++;
                        msg.Add("「イヤーッ！」「グワーッ！」ニンジャスレイヤーにアンブッシュ成功！");
                    }
                }

            }
            return msg;
        }

        public List<string> MoveNinjaSlayer()
        {
            var msg = new List<string>();
            if (NinjaSlayer.Damage > 0)
            {
                msg.Add("「スゥーッ！ハァーッ！」（" + NinjaSlayer.Damage.ToString() + "回休み)");
                NinjaSlayer.Damage--;
                return msg;
            }

            var Dice = 4;
            var next = 0;
            if (NinjaSlayer.IsShugi)
            {
                // 1マスごとにアンブッシュ判定
                for (int i = 0; i < Dice; i++)
                {
                    // 次のマスが4ならば0とする
                    next = (NinjaSlayer.PositionShugi + 1 == 4) ? 0 : NinjaSlayer.PositionShugi + 1;
                    if (MyPlayer.IsShugi && next == MyPlayer.PositionShugi)
                    {
                        // プレイヤーに追い付いた
                        MyPlayer.IsDead = true;
                        msg.Add("「イヤーッ！」「アバーッ！サヨナラ！」あなたは爆発四散！");
                    }
                    NinjaSlayer.PositionShugi = next;
                }

                // 開始地点ならばシュギ・ジキから出る
                if (NinjaSlayer.PositionShugi == NinjaSlayer.PositionShugiStart)
                {
                    NinjaSlayer.IsShugi = false;
                    msg.Add("ニンジャスレイヤーはシュギ・ジキから離脱した！");

                    if (MyPlayer.IsShugi == false && NinjaSlayer.PositionOuter == MyPlayer.PositionOuter)
                    {
                        // プレイヤーに追い付いた
                        MyPlayer.IsDead = true;
                        msg.Add("「イヤーッ！」「アバーッ！サヨナラ！」あなたは爆発四散！");
                    }
                }
            }
            else
            {
                // 1マスごとにアンブッシュ判定
                for (int i = 0; i < Dice; i++)
                {
                    // 次のマスが20ならば0とする
                    next = (NinjaSlayer.PositionOuter + 1 == 20) ? 0 : NinjaSlayer.PositionOuter + 1;
                    if (MyPlayer.IsShugi == false && next == MyPlayer.PositionOuter)
                    {
                        // プレイヤーに追い付いた
                        MyPlayer.IsDead = true;
                        msg.Add("「イヤーッ！」「アバーッ！サヨナラ！」あなたは爆発四散！");
                    }
                    NinjaSlayer.PositionOuter = next;
                }

                // 開始地点ならばシュギ・ジキに入る
                if (NinjaSlayer.PositionOuter % 5 == 0)
                {
                    NinjaSlayer.IsShugi = true;
                    NinjaSlayer.PositionShugiStart = NinjaSlayer.PositionOuter / 5;
                    NinjaSlayer.PositionShugi = NinjaSlayer.PositionShugiStart;
                    msg.Add("「バカな……行き止まりとは……！」");

                    if (MyPlayer.IsShugi && NinjaSlayer.PositionShugi == MyPlayer.PositionShugi)
                    {
                        // プレイヤーに追い付いた
                        MyPlayer.IsDead = true;
                        msg.Add("「イヤーッ！」「アバーッ！サヨナラ！」あなたは爆発四散！");
                    }
                }

            }
            return msg;

        }
    }
}
