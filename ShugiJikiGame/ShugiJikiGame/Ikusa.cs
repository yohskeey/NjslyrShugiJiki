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

        public Ikusa()
        {
            MyPlayer = new Player(false);
            NinjaSlayer = new Player(true);
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
                        NinjaSlayer.damage++;
                        MyPlayer.AmbushCount++;
                        msg.Add("「イヤーッ！」「グワーッ！」ニンジャスレイヤーにアンブッシュ成功！");
                    }
                    MyPlayer.PositionShugi = next;
                }
                
                // 開始地点ならばシュギ・ジキから出る
                if(MyPlayer.PositionShugi == MyPlayer.PositionShugiStart)
                {
                    MyPlayer.IsShugi = false;
                    msg.Add("シュギ・ジキから離脱した！");
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
                        NinjaSlayer.damage++;
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
                    msg.Add("シュギ・ジキに捕らわれた");
                }

            }
            return msg;
        }

        public List<string> MoveNinjaSlayer()
        {
            var msg = new List<string>();
            if (NinjaSlayer.damage > 0)
            {
                NinjaSlayer.damage--;
                msg.Add( "ニンジャスレイヤーはチャドー呼吸をしている");
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
                    msg.Add("「ヌウーッ…行き止まりとは」");
                }

            }
            return msg;

        }
    }
}
