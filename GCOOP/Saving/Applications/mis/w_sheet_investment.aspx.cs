using System;
using CoreSavingLibrary;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;


namespace Saving.Applications.mis
{
    public class cal_c
    {
        public int pv(int val, int num)
        {
            int pre;
            pre = val * num;
            return pre;
        }

        public double fv(int val, int num, int yea, double ii, int perr)
        {
            double fu = 0, iii, iii6, iii3;

            iii = ii / 100;
            iii3 = (Math.Pow(((iii / 4) + 1), 4)) - 1;
            iii6 = (Math.Pow(((iii / 2) + 1), 2)) - 1;

            if (perr == 12)
            {
                fu = Math.Pow((1 + iii), yea);
                fu = fu * (val * num);
            }
            else if (perr == 6)
            {
                fu = Math.Pow((1 + iii6), yea);
                fu = fu * (val * num);
            }
            else if (perr == 3)
            {
                fu = Math.Pow((1 + iii3), yea);
                fu = fu * (val * num);
            }
            return fu;
        }

        public double npv(int val, int num, int yea, double ii, double npvi, int perr)
        {
            double getnpv = 0, iii, iii6, iii3, iinpv;

            iii = ii / 100;
            iii3 = (Math.Pow(((iii / 4) + 1), 4)) - 1;
            iii6 = (Math.Pow(((iii / 2) + 1), 2)) - 1;
            iinpv = npvi / 100;

            if (perr == 12)
            {
                getnpv = (Math.Pow(((val * num) * (1 + iii)), yea) / (1 + iinpv)) - (val * num);
            }
            else if (perr == 6)
            {
                getnpv = (Math.Pow(((val * num) * (1 + iii6)), yea) / (1 + iinpv)) - (val * num);
            }
            else if (perr == 3)
            {
                getnpv = (Math.Pow(((val * num) * (1 + iii3)), yea) / (1 + iinpv)) - (val * num);
            }

            return getnpv;
        }

        public double now(double Mnow, int val)
        {
            double m;
            m = Mnow - val;
            return m;
        }

        public double irr(double ii, int perr)
        {
            double getIRR = 0, iii;

            iii = ii / 100;

            if (perr == 12)
            {
                getIRR = ii / 100;
            }
            else if (perr == 3)
            {
                getIRR = (Math.Pow(((iii / 4) + 1), 4)) - 1;
            }
            else if (perr == 6)
            {
                getIRR = (Math.Pow(((iii / 2) + 1), 2)) - 1;
            }

            return getIRR;
        }

        public double fvT(int val, double ii, int yea)
        {
            double fuT, iT;

            iT = ii / 100;
            iT = iT / (12 / yea);
            fuT = (val) * (1 + iT);
            return fuT;
        }

        public double npvT(int val, double ii, double npvi, int yea)
        {
            double getnpvT, inpvT, iT;

            inpvT = npvi / 100;
            inpvT = inpvT / 12;
            inpvT = inpvT * yea;

            iT = ii / 100;
            iT = iT / (12 / yea);

            getnpvT = ((val * (1 + iT)) / (1 + inpvT)) - val;
            return getnpvT;
        }

        public double irrT(double ii, int yea)
        {
            double iT;

            iT = ii / 100;
            iT = iT / (12 / yea);
            return iT;
        }

        public double fvP3(int Mget, double i1per, int nu1m, double i2per, int nu2m, double i3per, int nu3m, double getM, int numG)
        {
            double fuP3, ii1per, ii2per, ii3per, getMM;

            ii1per = i1per / 100;
            ii2per = i2per / 100;
            ii3per = i3per / 100;
            getMM = getM / 100;

            fuP3 = (Mget * ii1per * nu1m) + (Mget * ii2per * nu2m) + (Mget * ii3per * nu3m) + (Mget * getMM * numG);

            return fuP3;
        }

        public double nowP(int Pyear, int yearP, double Mnow)
        {
            double Pnow;

            Pnow = Mnow - (Pyear * yearP);
            return Pnow;
        }

        public double last(double ilast, double moneylast, int Mget)
        {
            double lastAll, lastii;

            lastii = ilast / 100;
            lastii = lastii * Mget;
            lastAll = moneylast + lastii;
            return lastAll;
        }

        public double npv1(int Mget, int numi, double i1per, double i2per, double i3per, int nu1m, int nu2m, int nu3m, int yearP, int Pyear, double npvi, int yearG, int yea1r, int yea2r, int yea3r, int en1d, int en2d, int en3d, double ilast, double moneylast, int star1t, int star2t, int star3t)
        {
            double NonNpv, lastinpv, lastmoneynpv, P1, PP1, PPP1, R1, RR1, RRR1;
            double NPVE1 = 0, NPV1 = 0, NPVE2 = 0, NPV2 = 0, NPVE3 = 0, NPV3 = 0;
            double npvAll, lastii, npvii;
            double ii1per, ii2per, ii3per;

            ii1per = i1per / 100;
            ii2per = i2per / 100;
            ii3per = i3per / 100;
            lastii = ilast / 100;
            lastii = lastii * Mget;
            npvii = npvi / 100;

            NonNpv = (Pyear * (((Math.Pow((1 + npvii), yearP)) - 1) / (npvii * Math.Pow((1 + npvii), yearP))));
            lastinpv = lastii / (Math.Pow((1 + npvii), yearG));
            lastmoneynpv = moneylast / (Math.Pow((1 + npvii), yearG));

            if (numi == 1)
            {
                if (star1t == nu1m)
                {
                    P1 = Mget * ii1per;
                    PP1 = npvii / ((Math.Pow((1 + npvii), yea1r)) - 1);
                    PPP1 = 1 / (Math.Pow((1 + npvii), en1d));
                    NPVE1 = P1 * PP1 * PPP1;
                }
                else if (star1t != nu1m)
                {
                    R1 = Mget * ii1per;
                    RR1 = npvii / ((Math.Pow((1 + npvii), yea1r)) - 1);
                    RRR1 = ((Math.Pow((1 + npvii), (yea1r * nu1m))) - 1) / (npvii * (Math.Pow((1 + npvii), en1d)));
                    NPV1 = R1 * RR1 * RRR1;
                }
            }

            if (numi == 2)
            {
                if (star1t == nu1m)
                {
                    P1 = Mget * ii1per;
                    PP1 = npvii / ((Math.Pow((1 + npvii), yea1r)) - 1);
                    PPP1 = 1 / (Math.Pow((1 + npvii), en1d));
                    NPVE1 = P1 * PP1 * PPP1;
                }
                else if (star1t != nu1m)
                {
                    R1 = Mget * ii1per;
                    RR1 = npvii / ((Math.Pow((1 + npvii), yea1r)) - 1);
                    RRR1 = ((Math.Pow((1 + npvii), (yea1r * nu1m))) - 1) / (npvii * (Math.Pow((1 + npvii), en1d)));
                    NPV1 = R1 * RR1 * RRR1;
                }

                if (star2t == nu2m)
                {
                    P1 = Mget * ii2per;
                    PP1 = npvii / ((Math.Pow((1 + npvii), yea2r)) - 1);
                    PPP1 = 1 / (Math.Pow((1 + npvii), en2d));
                    NPVE2 = P1 * PP1 * PPP1;
                }
                else if (star2t != nu2m)
                {
                    R1 = Mget * ii2per;
                    RR1 = npvii / ((Math.Pow((1 + npvii), yea2r)) - 1);
                    RRR1 = ((Math.Pow((1 + npvii), (yea2r * nu2m))) - 1) / (npvii * (Math.Pow((1 + npvii), en2d)));
                    NPV2 = R1 * RR1 * RRR1;
                }

            }

            if (numi == 3)
            {
                if (star1t == nu1m)
                {
                    P1 = Mget * ii1per;
                    PP1 = npvii / ((Math.Pow((1 + npvii), yea1r)) - 1);
                    PPP1 = 1 / (Math.Pow((1 + npvii), en1d));
                    NPVE1 = P1 * PP1 * PPP1;
                }
                else if (star1t != nu1m)
                {
                    R1 = Mget * ii1per;
                    RR1 = npvii / ((Math.Pow((1 + npvii), yea1r)) - 1);
                    RRR1 = ((Math.Pow((1 + npvii), (yea1r * nu1m))) - 1) / (npvii * (Math.Pow((1 + npvii), en1d)));
                    NPV1 = R1 * RR1 * RRR1;
                }

                if (star2t == nu2m)
                {
                    P1 = Mget * ii2per;
                    PP1 = npvii / ((Math.Pow((1 + npvii), yea2r)) - 1);
                    PPP1 = 1 / (Math.Pow((1 + npvii), en2d));
                    NPVE2 = P1 * PP1 * PPP1;
                }
                else if (star2t != nu2m)
                {
                    R1 = Mget * ii2per;
                    RR1 = npvii / ((Math.Pow((1 + npvii), yea2r)) - 1);
                    RRR1 = ((Math.Pow((1 + npvii), (yea2r * nu2m))) - 1) / (npvii * (Math.Pow((1 + npvii), en2d)));
                    NPV2 = R1 * RR1 * RRR1;
                }

                if (star3t == nu3m)
                {
                    P1 = Mget * ii3per;
                    PP1 = npvii / ((Math.Pow((1 + npvii), yea3r)) - 1);
                    PPP1 = 1 / (Math.Pow((1 + npvii), en3d));
                    NPVE3 = P1 * PP1 * PPP1;
                }
                else if (star3t != nu3m)
                {
                    R1 = Mget * ii3per;
                    RR1 = npvii / ((Math.Pow((1 + npvii), yea3r)) - 1);
                    RRR1 = ((Math.Pow((1 + npvii), (yea3r * nu3m))) - 1) / (npvii * (Math.Pow((1 + npvii), en3d)));
                    NPV3 = R1 * RR1 * RRR1;
                }
            }
            npvAll = (NPVE1 + NPV1 + NPVE2 + NPV2 + NPVE3 + NPV3 + lastinpv + lastmoneynpv) - NonNpv;
            return npvAll;
        }
    }


    public partial class w_sheet_investment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            MultiView1.SetActiveView(View1);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            MultiView1.SetActiveView(View2);
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            MultiView1.SetActiveView(View3);
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            MultiView1.SetActiveView(View4);
        }


        protected void Button13_Click(object sender, EventArgs e)
        {
            int count1, count2, count3, count4;

            TextBox57.Text = TextBox49.Text;
            MultiView1.Visible = true;
            Label1.Visible = true;
            Label2.Visible = true;
            TextBox57.Visible = true;
            Button1.Visible = true;
            Button2.Visible = true;
            Button3.Visible = true;
            Button4.Visible = true;
            count1 = Convert.ToInt16(TextBox58.Text);
            count2 = Convert.ToInt16(TextBox59.Text);
            count3 = Convert.ToInt16(TextBox60.Text);
            count4 = Convert.ToInt16(TextBox61.Text);

            if (count1 == 1)
            {
                TextBox62.Visible = true;
                TextBox63.Visible = true;
                TextBox64.Visible = true;
                TextBox65.Visible = true;

                TextBox66.Visible = false;
                TextBox70.Visible = false;
                TextBox74.Visible = false;
                TextBox78.Visible = false;

                TextBox67.Visible = false;
                TextBox71.Visible = false;
                TextBox75.Visible = false;
                TextBox79.Visible = false;

                TextBox68.Visible = false;
                TextBox72.Visible = false;
                TextBox76.Visible = false;
                TextBox80.Visible = false;

                TextBox69.Visible = false;
                TextBox73.Visible = false;
                TextBox77.Visible = false;
                TextBox81.Visible = false;
            }
            else if (count1 == 2)
            {
                TextBox62.Visible = true;
                TextBox63.Visible = true;
                TextBox64.Visible = true;
                TextBox65.Visible = true;

                TextBox66.Visible = true;
                TextBox70.Visible = true;
                TextBox74.Visible = true;
                TextBox78.Visible = true;

                TextBox67.Visible = false;
                TextBox71.Visible = false;
                TextBox75.Visible = false;
                TextBox79.Visible = false;

                TextBox68.Visible = false;
                TextBox72.Visible = false;
                TextBox76.Visible = false;
                TextBox80.Visible = false;

                TextBox69.Visible = false;
                TextBox73.Visible = false;
                TextBox77.Visible = false;
                TextBox81.Visible = false;
            }
            else if (count1 == 3)
            {
                TextBox62.Visible = true;
                TextBox63.Visible = true;
                TextBox64.Visible = true;
                TextBox65.Visible = true;

                TextBox66.Visible = true;
                TextBox70.Visible = true;
                TextBox74.Visible = true;
                TextBox78.Visible = true;

                TextBox67.Visible = true;
                TextBox71.Visible = true;
                TextBox75.Visible = true;
                TextBox79.Visible = true;

                TextBox68.Visible = false;
                TextBox72.Visible = false;
                TextBox76.Visible = false;
                TextBox80.Visible = false;

                TextBox69.Visible = false;
                TextBox73.Visible = false;
                TextBox77.Visible = false;
                TextBox81.Visible = false;
            }
            else if (count1 == 4)
            {
                TextBox62.Visible = true;
                TextBox63.Visible = true;
                TextBox64.Visible = true;
                TextBox65.Visible = true;

                TextBox66.Visible = true;
                TextBox70.Visible = true;
                TextBox74.Visible = true;
                TextBox78.Visible = true;

                TextBox67.Visible = true;
                TextBox71.Visible = true;
                TextBox75.Visible = true;
                TextBox79.Visible = true;

                TextBox68.Visible = true;
                TextBox72.Visible = true;
                TextBox76.Visible = true;
                TextBox80.Visible = true;

                TextBox69.Visible = false;
                TextBox73.Visible = false;
                TextBox77.Visible = false;
                TextBox81.Visible = false;
            }
            else if (count1 == 5)
            {
                TextBox62.Visible = true;
                TextBox63.Visible = true;
                TextBox64.Visible = true;
                TextBox65.Visible = true;

                TextBox66.Visible = true;
                TextBox70.Visible = true;
                TextBox74.Visible = true;
                TextBox78.Visible = true;

                TextBox67.Visible = true;
                TextBox71.Visible = true;
                TextBox75.Visible = true;
                TextBox79.Visible = true;

                TextBox68.Visible = true;
                TextBox72.Visible = true;
                TextBox76.Visible = true;
                TextBox80.Visible = true;

                TextBox69.Visible = true;
                TextBox73.Visible = true;
                TextBox77.Visible = true;
                TextBox81.Visible = true;
            }

            if (count2 == 1)
            {
                TextBox82.Visible = true;
                TextBox83.Visible = true;
                TextBox84.Visible = true;
                TextBox85.Visible = true;

                TextBox86.Visible = false;
                TextBox87.Visible = false;
                TextBox88.Visible = false;
                TextBox89.Visible = false;

                TextBox90.Visible = false;
                TextBox91.Visible = false;
                TextBox92.Visible = false;
                TextBox93.Visible = false;

                TextBox94.Visible = false;
                TextBox95.Visible = false;
                TextBox96.Visible = false;
                TextBox97.Visible = false;

                TextBox98.Visible = false;
                TextBox99.Visible = false;
                TextBox100.Visible = false;
                TextBox101.Visible = false;
            }
            else if (count2 == 2)
            {
                TextBox82.Visible = true;
                TextBox83.Visible = true;
                TextBox84.Visible = true;
                TextBox85.Visible = true;

                TextBox86.Visible = true;
                TextBox87.Visible = true;
                TextBox88.Visible = true;
                TextBox89.Visible = true;

                TextBox90.Visible = false;
                TextBox91.Visible = false;
                TextBox92.Visible = false;
                TextBox93.Visible = false;

                TextBox94.Visible = false;
                TextBox95.Visible = false;
                TextBox96.Visible = false;
                TextBox97.Visible = false;

                TextBox98.Visible = false;
                TextBox99.Visible = false;
                TextBox100.Visible = false;
                TextBox101.Visible = false;
            }
            else if (count2 == 3)
            {
                TextBox82.Visible = true;
                TextBox83.Visible = true;
                TextBox84.Visible = true;
                TextBox85.Visible = true;

                TextBox86.Visible = true;
                TextBox87.Visible = true;
                TextBox88.Visible = true;
                TextBox89.Visible = true;

                TextBox90.Visible = true;
                TextBox91.Visible = true;
                TextBox92.Visible = true;
                TextBox93.Visible = true;

                TextBox94.Visible = false;
                TextBox95.Visible = false;
                TextBox96.Visible = false;
                TextBox97.Visible = false;

                TextBox98.Visible = false;
                TextBox99.Visible = false;
                TextBox100.Visible = false;
                TextBox101.Visible = false;
            }
            else if (count2 == 4)
            {
                TextBox82.Visible = true;
                TextBox83.Visible = true;
                TextBox84.Visible = true;
                TextBox85.Visible = true;

                TextBox86.Visible = true;
                TextBox87.Visible = true;
                TextBox88.Visible = true;
                TextBox89.Visible = true;

                TextBox90.Visible = true;
                TextBox91.Visible = true;
                TextBox92.Visible = true;
                TextBox93.Visible = true;

                TextBox94.Visible = true;
                TextBox95.Visible = true;
                TextBox96.Visible = true;
                TextBox97.Visible = true;

                TextBox98.Visible = false;
                TextBox99.Visible = false;
                TextBox100.Visible = false;
                TextBox101.Visible = false;
            }
            else if (count2 == 5)
            {
                TextBox82.Visible = true;
                TextBox83.Visible = true;
                TextBox84.Visible = true;
                TextBox85.Visible = true;

                TextBox86.Visible = true;
                TextBox87.Visible = true;
                TextBox88.Visible = true;
                TextBox89.Visible = true;

                TextBox90.Visible = true;
                TextBox91.Visible = true;
                TextBox92.Visible = true;
                TextBox93.Visible = true;

                TextBox94.Visible = true;
                TextBox95.Visible = true;
                TextBox96.Visible = true;
                TextBox97.Visible = true;

                TextBox98.Visible = true;
                TextBox99.Visible = true;
                TextBox100.Visible = true;
                TextBox101.Visible = true;
            }

            if (count3 == 1)
            {
                TextBox102.Visible = true;
                TextBox103.Visible = true;
                TextBox104.Visible = true;
                TextBox105.Visible = true;

                TextBox106.Visible = false;
                TextBox107.Visible = false;
                TextBox108.Visible = false;
                TextBox109.Visible = false;

                TextBox110.Visible = false;
                TextBox111.Visible = false;
                TextBox112.Visible = false;
                TextBox113.Visible = false;

                TextBox114.Visible = false;
                TextBox115.Visible = false;
                TextBox116.Visible = false;
                TextBox117.Visible = false;

                TextBox118.Visible = false;
                TextBox119.Visible = false;
                TextBox120.Visible = false;
                TextBox121.Visible = false;
            }
            else if (count3 == 2)
            {
                TextBox102.Visible = true;
                TextBox103.Visible = true;
                TextBox104.Visible = true;
                TextBox105.Visible = true;

                TextBox106.Visible = true;
                TextBox107.Visible = true;
                TextBox108.Visible = true;
                TextBox109.Visible = true;

                TextBox110.Visible = false;
                TextBox111.Visible = false;
                TextBox112.Visible = false;
                TextBox113.Visible = false;

                TextBox114.Visible = false;
                TextBox115.Visible = false;
                TextBox116.Visible = false;
                TextBox117.Visible = false;

                TextBox118.Visible = false;
                TextBox119.Visible = false;
                TextBox120.Visible = false;
                TextBox121.Visible = false;
            }
            else if (count3 == 3)
            {
                TextBox102.Visible = true;
                TextBox103.Visible = true;
                TextBox104.Visible = true;
                TextBox105.Visible = true;

                TextBox106.Visible = true;
                TextBox107.Visible = true;
                TextBox108.Visible = true;
                TextBox109.Visible = true;

                TextBox110.Visible = true;
                TextBox111.Visible = true;
                TextBox112.Visible = true;
                TextBox113.Visible = true;

                TextBox114.Visible = false;
                TextBox115.Visible = false;
                TextBox116.Visible = false;
                TextBox117.Visible = false;

                TextBox118.Visible = false;
                TextBox119.Visible = false;
                TextBox120.Visible = false;
                TextBox121.Visible = false;
            }
            else if (count3 == 4)
            {
                TextBox102.Visible = true;
                TextBox103.Visible = true;
                TextBox104.Visible = true;
                TextBox105.Visible = true;

                TextBox106.Visible = true;
                TextBox107.Visible = true;
                TextBox108.Visible = true;
                TextBox109.Visible = true;

                TextBox110.Visible = true;
                TextBox111.Visible = true;
                TextBox112.Visible = true;
                TextBox113.Visible = true;

                TextBox114.Visible = true;
                TextBox115.Visible = true;
                TextBox116.Visible = true;
                TextBox117.Visible = true;

                TextBox118.Visible = false;
                TextBox119.Visible = false;
                TextBox120.Visible = false;
                TextBox121.Visible = false;
            }
            else if (count3 == 5)
            {
                TextBox102.Visible = true;
                TextBox103.Visible = true;
                TextBox104.Visible = true;
                TextBox105.Visible = true;

                TextBox106.Visible = true;
                TextBox107.Visible = true;
                TextBox108.Visible = true;
                TextBox109.Visible = true;

                TextBox110.Visible = true;
                TextBox111.Visible = true;
                TextBox112.Visible = true;
                TextBox113.Visible = true;

                TextBox114.Visible = true;
                TextBox115.Visible = true;
                TextBox116.Visible = true;
                TextBox117.Visible = true;

                TextBox118.Visible = true;
                TextBox119.Visible = true;
                TextBox120.Visible = true;
                TextBox121.Visible = true;
            }

            if (count4 == 1)
            {
                TextBox122.Visible = true;
                TextBox46.Visible = true;
                TextBox48.Visible = true;
                TextBox47.Visible = true;

                TextBox126.Visible = false;
                TextBox127.Visible = false;
                TextBox128.Visible = false;
                TextBox129.Visible = false;

                TextBox130.Visible = false;
                TextBox131.Visible = false;
                TextBox132.Visible = false;
                TextBox133.Visible = false;

                TextBox134.Visible = false;
                TextBox135.Visible = false;
                TextBox136.Visible = false;
                TextBox137.Visible = false;

                TextBox138.Visible = false;
                TextBox139.Visible = false;
                TextBox140.Visible = false;
                TextBox141.Visible = false;
            }
            else if (count4 == 2)
            {
                TextBox122.Visible = true;
                TextBox46.Visible = true;
                TextBox48.Visible = true;
                TextBox47.Visible = true;

                TextBox126.Visible = true;
                TextBox127.Visible = true;
                TextBox128.Visible = true;
                TextBox129.Visible = true;

                TextBox130.Visible = false;
                TextBox131.Visible = false;
                TextBox132.Visible = false;
                TextBox133.Visible = false;

                TextBox134.Visible = false;
                TextBox135.Visible = false;
                TextBox136.Visible = false;
                TextBox137.Visible = false;

                TextBox138.Visible = false;
                TextBox139.Visible = false;
                TextBox140.Visible = false;
                TextBox141.Visible = false;
            }
            else if (count4 == 3)
            {
                TextBox122.Visible = true;
                TextBox46.Visible = true;
                TextBox48.Visible = true;
                TextBox47.Visible = true;

                TextBox126.Visible = true;
                TextBox127.Visible = true;
                TextBox128.Visible = true;
                TextBox129.Visible = true;

                TextBox130.Visible = true;
                TextBox131.Visible = true;
                TextBox132.Visible = true;
                TextBox133.Visible = true;

                TextBox134.Visible = false;
                TextBox135.Visible = false;
                TextBox136.Visible = false;
                TextBox137.Visible = false;

                TextBox138.Visible = false;
                TextBox139.Visible = false;
                TextBox140.Visible = false;
                TextBox141.Visible = false;
            }
            else if (count4 == 4)
            {
                TextBox122.Visible = true;
                TextBox46.Visible = true;
                TextBox48.Visible = true;
                TextBox47.Visible = true;

                TextBox126.Visible = true;
                TextBox127.Visible = true;
                TextBox128.Visible = true;
                TextBox129.Visible = true;

                TextBox130.Visible = true;
                TextBox131.Visible = true;
                TextBox132.Visible = true;
                TextBox133.Visible = true;

                TextBox134.Visible = true;
                TextBox135.Visible = true;
                TextBox136.Visible = true;
                TextBox137.Visible = true;

                TextBox138.Visible = false;
                TextBox139.Visible = false;
                TextBox140.Visible = false;
                TextBox141.Visible = false;
            }
            else if (count4 == 5)
            {
                TextBox122.Visible = true;
                TextBox46.Visible = true;
                TextBox48.Visible = true;
                TextBox47.Visible = true;

                TextBox126.Visible = true;
                TextBox127.Visible = true;
                TextBox128.Visible = true;
                TextBox129.Visible = true;

                TextBox130.Visible = true;
                TextBox131.Visible = true;
                TextBox132.Visible = true;
                TextBox133.Visible = true;

                TextBox134.Visible = true;
                TextBox135.Visible = true;
                TextBox136.Visible = true;
                TextBox137.Visible = true;

                TextBox138.Visible = true;
                TextBox139.Visible = true;
                TextBox140.Visible = true;
                TextBox141.Visible = true;
            }
        }

        protected void DropDownList4_SelectedIndexChanged(object sender, EventArgs e)
        {
            int inum;
            inum = Convert.ToInt32(DropDownList4.Text);

            if (inum == 1)
            {
                TextBox30.ReadOnly = false;
                TextBox18.ReadOnly = false;
                TextBox21.ReadOnly = false;
                TextBox26.ReadOnly = false;
                TextBox27.ReadOnly = false;
                TextBox16.ReadOnly = true;
                TextBox19.ReadOnly = true;
                TextBox22.ReadOnly = true;
                TextBox25.ReadOnly = true;
                TextBox28.ReadOnly = true;
                TextBox17.ReadOnly = true;
                TextBox20.ReadOnly = true;
                TextBox23.ReadOnly = true;
                TextBox24.ReadOnly = true;
                TextBox29.ReadOnly = true;

                TextBox30.Text = "";
                TextBox18.Text = "";
                TextBox21.Text = "";
                TextBox26.Text = "";
                TextBox27.Text = "";
                TextBox16.Text = 0 + "";
                TextBox19.Text = 0 + "";
                TextBox22.Text = 0 + "";
                TextBox25.Text = 0 + "";
                TextBox28.Text = 0 + "";
                TextBox17.Text = 0 + "";
                TextBox20.Text = 0 + "";
                TextBox23.Text = 0 + "";
                TextBox24.Text = 0 + "";
                TextBox29.Text = 0 + "";
            }
            else if (inum == 2)
            {
                TextBox30.ReadOnly = false;
                TextBox18.ReadOnly = false;
                TextBox21.ReadOnly = false;
                TextBox26.ReadOnly = false;
                TextBox27.ReadOnly = false;
                TextBox16.ReadOnly = false;
                TextBox19.ReadOnly = false;
                TextBox22.ReadOnly = false;
                TextBox25.ReadOnly = false;
                TextBox28.ReadOnly = false;
                TextBox17.ReadOnly = true;
                TextBox20.ReadOnly = true;
                TextBox23.ReadOnly = true;
                TextBox24.ReadOnly = true;
                TextBox29.ReadOnly = true;

                TextBox30.Text = "";
                TextBox18.Text = "";
                TextBox21.Text = "";
                TextBox26.Text = "";
                TextBox27.Text = "";
                TextBox16.Text = "";
                TextBox19.Text = "";
                TextBox22.Text = "";
                TextBox25.Text = "";
                TextBox28.Text = "";
                TextBox17.Text = 0 + "";
                TextBox20.Text = 0 + "";
                TextBox23.Text = 0 + "";
                TextBox24.Text = 0 + "";
                TextBox29.Text = 0 + "";
            }
            else if (inum == 3)
            {
                TextBox30.ReadOnly = false;
                TextBox18.ReadOnly = false;
                TextBox21.ReadOnly = false;
                TextBox26.ReadOnly = false;
                TextBox27.ReadOnly = false;
                TextBox16.ReadOnly = false;
                TextBox19.ReadOnly = false;
                TextBox22.ReadOnly = false;
                TextBox25.ReadOnly = false;
                TextBox28.ReadOnly = false;
                TextBox17.ReadOnly = false;
                TextBox20.ReadOnly = false;
                TextBox23.ReadOnly = false;
                TextBox24.ReadOnly = false;
                TextBox29.ReadOnly = false;

                TextBox30.Text = "";
                TextBox18.Text = "";
                TextBox21.Text = "";
                TextBox26.Text = "";
                TextBox27.Text = "";
                TextBox16.Text = "";
                TextBox19.Text = "";
                TextBox22.Text = "";
                TextBox25.Text = "";
                TextBox28.Text = "";
                TextBox17.Text = "";
                TextBox20.Text = "";
                TextBox23.Text = "";
                TextBox24.Text = "";
                TextBox29.Text = "";
            }
        }

        protected void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            TextBox32.Text = 0 + "";
            TextBox32.ReadOnly = true;
            TextBox31.Text = "";
            TextBox31.ReadOnly = false;
        }

        protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            TextBox31.Text = 0 + "";
            TextBox31.ReadOnly = true;
            TextBox32.Text = "";
            TextBox32.ReadOnly = false;
        }


        protected void Button5_Click(object sender, EventArgs e)
        {
            int value, number, year, per, x;
            double i, inpv, y, nowM;
            cal_c calc = new cal_c();

            value = Convert.ToInt32(TextBox1.Text);
            number = Convert.ToInt32(TextBox2.Text);
            year = Convert.ToInt32(TextBox3.Text);
            per = Convert.ToInt32(DropDownList1.Text);
            i = Convert.ToDouble(TextBox4.Text);
            inpv = Convert.ToDouble(TextBox56.Text);
            nowM = Convert.ToDouble(TextBox57.Text);

            if (TextBox62.Text == "")
            {
                x = calc.pv(value, number);
                TextBox62.Text = x + "";
                y = calc.now(nowM, value);
                TextBox57.Text = y + "";
                y = calc.npv(value, number, year, i, inpv, per);
                TextBox64.Text = y + "";
                y = calc.fv(value, number, year, i, per);
                TextBox63.Text = y + "";
                y = calc.irr(i, per);
                TextBox65.Text = y + "";
            }
            else if ((TextBox62.Text != "") && (TextBox66.Text == ""))
            {
                x = calc.pv(value, number);
                TextBox66.Text = x + "";
                y = calc.now(nowM, value);
                TextBox57.Text = y + "";
                y = calc.npv(value, number, year, i, inpv, per);
                TextBox74.Text = y + "";
                y = calc.fv(value, number, year, i, per);
                TextBox70.Text = y + "";
                y = calc.irr(i, per);
                TextBox78.Text = y + "";
            }
            else if ((TextBox66.Text != "") && (TextBox67.Text == ""))
            {
                x = calc.pv(value, number);
                TextBox67.Text = x + "";
                y = calc.now(nowM, value);
                TextBox57.Text = y + "";
                y = calc.npv(value, number, year, i, inpv, per);
                TextBox75.Text = y + "";
                y = calc.fv(value, number, year, i, per);
                TextBox71.Text = y + "";
                y = calc.irr(i, per);
                TextBox79.Text = y + "";
            }
            else if ((TextBox67.Text != "") && (TextBox68.Text == ""))
            {
                x = calc.pv(value, number);
                TextBox68.Text = x + "";
                y = calc.now(nowM, value);
                TextBox57.Text = y + "";
                y = calc.npv(value, number, year, i, inpv, per);
                TextBox76.Text = y + "";
                y = calc.fv(value, number, year, i, per);
                TextBox72.Text = y + "";
                y = calc.irr(i, per);
                TextBox80.Text = y + "";
            }
            else if ((TextBox68.Text != "") && (TextBox69.Text == ""))
            {
                x = calc.pv(value, number);
                TextBox69.Text = x + "";
                y = calc.now(nowM, value);
                TextBox57.Text = y + "";
                y = calc.npv(value, number, year, i, inpv, per);
                TextBox77.Text = y + "";
                y = calc.fv(value, number, year, i, per);
                TextBox73.Text = y + "";
                y = calc.irr(i, per);
                TextBox81.Text = y + "";
            }

        }

        protected void Button7_Click(object sender, EventArgs e)
        {
            int value, year, x;
            double i, inpv, y, nowM;
            cal_c calc = new cal_c();

            value = Convert.ToInt32(TextBox9.Text);
            year = Convert.ToInt32(DropDownList3.Text);
            i = Convert.ToDouble(TextBox10.Text);
            inpv = Convert.ToDouble(TextBox56.Text);
            nowM = Convert.ToDouble(TextBox57.Text);

            if (TextBox82.Text == "")
            {
                TextBox82.Text = value + "";
                y = calc.now(nowM, value);
                TextBox57.Text = y + "";
                y = calc.fvT(value, i, year);
                TextBox83.Text = y + "";
                y = calc.npvT(value, i, inpv, year);
                TextBox84.Text = y + "";
                y = calc.irrT(i, year);
                TextBox85.Text = y + "";
            }
            else if ((TextBox82.Text != "") && (TextBox86.Text == ""))
            {
                TextBox86.Text = value + "";
                y = calc.now(nowM, value);
                TextBox57.Text = y + "";
                y = calc.fvT(value, i, year);
                TextBox87.Text = y + "";
                y = calc.npvT(value, i, inpv, year);
                TextBox88.Text = y + "";
                y = calc.irrT(i, year);
                TextBox89.Text = y + "";
            }
            else if ((TextBox86.Text != "") && (TextBox90.Text == ""))
            {
                TextBox90.Text = value + "";
                y = calc.now(nowM, value);
                TextBox57.Text = y + "";
                y = calc.fvT(value, i, year);
                TextBox91.Text = y + "";
                y = calc.npvT(value, i, inpv, year);
                TextBox92.Text = y + "";
                y = calc.irrT(i, year);
                TextBox93.Text = y + "";
            }
            else if ((TextBox90.Text != "") && (TextBox94.Text == ""))
            {
                TextBox94.Text = value + "";
                y = calc.now(nowM, value);
                TextBox57.Text = y + "";
                y = calc.fvT(value, i, year);
                TextBox95.Text = y + "";
                y = calc.npvT(value, i, inpv, year);
                TextBox96.Text = y + "";
                y = calc.irrT(i, year);
                TextBox97.Text = y + "";
            }
            else if ((TextBox94.Text != "") && (TextBox98.Text == ""))
            {
                TextBox98.Text = value + "";
                y = calc.now(nowM, value);
                TextBox57.Text = y + "";
                y = calc.fvT(value, i, year);
                TextBox99.Text = y + "";
                y = calc.npvT(value, i, inpv, year);
                TextBox100.Text = y + "";
                y = calc.irrT(i, year);
                TextBox101.Text = y + "";
            }
        }

        protected void Button9_Click(object sender, EventArgs e)
        {
            int value, number, year, per, x;
            double i, inpv, y, nowM;
            cal_c calc = new cal_c();

            value = Convert.ToInt32(TextBox5.Text);
            number = Convert.ToInt32(TextBox6.Text);
            year = Convert.ToInt32(TextBox7.Text);
            per = Convert.ToInt32(DropDownList2.Text);
            i = Convert.ToDouble(TextBox8.Text);
            inpv = Convert.ToDouble(TextBox56.Text);
            nowM = Convert.ToDouble(TextBox57.Text);

            if (TextBox102.Text == "")
            {
                x = calc.pv(value, number);
                TextBox102.Text = x + "";
                y = calc.now(nowM, value);
                TextBox57.Text = y + "";
                y = calc.npv(value, number, year, i, inpv, per);
                TextBox104.Text = y + "";
                y = calc.fv(value, number, year, i, per);
                TextBox103.Text = y + "";
                y = calc.irr(i, per);
                TextBox105.Text = y + "";
            }
            else if ((TextBox102.Text != "") && (TextBox106.Text == ""))
            {
                x = calc.pv(value, number);
                TextBox106.Text = x + "";
                y = calc.now(nowM, value);
                TextBox57.Text = y + "";
                y = calc.npv(value, number, year, i, inpv, per);
                TextBox108.Text = y + "";
                y = calc.fv(value, number, year, i, per);
                TextBox107.Text = y + "";
                y = calc.irr(i, per);
                TextBox109.Text = y + "";
            }
            else if ((TextBox106.Text != "") && (TextBox110.Text == ""))
            {
                x = calc.pv(value, number);
                TextBox110.Text = x + "";
                y = calc.now(nowM, value);
                TextBox57.Text = y + "";
                y = calc.npv(value, number, year, i, inpv, per);
                TextBox112.Text = y + "";
                y = calc.fv(value, number, year, i, per);
                TextBox111.Text = y + "";
                y = calc.irr(i, per);
                TextBox113.Text = y + "";
            }
            else if ((TextBox110.Text != "") && (TextBox114.Text == ""))
            {
                x = calc.pv(value, number);
                TextBox114.Text = x + "";
                y = calc.now(nowM, value);
                TextBox57.Text = y + "";
                y = calc.npv(value, number, year, i, inpv, per);
                TextBox116.Text = y + "";
                y = calc.fv(value, number, year, i, per);
                TextBox115.Text = y + "";
                y = calc.irr(i, per);
                TextBox117.Text = y + "";
            }
            else if ((TextBox114.Text != "") && (TextBox118.Text == ""))
            {
                x = calc.pv(value, number);
                TextBox118.Text = x + "";
                y = calc.now(nowM, value);
                TextBox57.Text = y + "";
                y = calc.npv(value, number, year, i, inpv, per);
                TextBox120.Text = y + "";
                y = calc.fv(value, number, year, i, per);
                TextBox119.Text = y + "";
                y = calc.irr(i, per);
                TextBox121.Text = y + "";
            }
        }

        protected void Button11_Click(object sender, EventArgs e)
        {
            int moneyget, payyear, yearpay, yearget, inum, numget, startnext, endnext;
            double iper1, iper2, iper3, lasti, lastmoney, getmoney, yearnext;
            int year1, year2, year3, num1, num2, num3, start1, start2, start3, end1, end2, end3;
            double x, y, z, inpv, nowM;
            cal_c calc = new cal_c();

            moneyget = Convert.ToInt32(TextBox11.Text);
            payyear = Convert.ToInt32(TextBox12.Text);
            yearpay = Convert.ToInt32(TextBox13.Text);
            yearget = Convert.ToInt32(TextBox14.Text);
            inum = Convert.ToInt32(DropDownList4.Text);
            iper1 = Convert.ToDouble(TextBox30.Text);
            iper2 = Convert.ToDouble(TextBox16.Text);
            iper3 = Convert.ToDouble(TextBox17.Text);
            year1 = Convert.ToInt32(TextBox18.Text);
            year2 = Convert.ToInt32(TextBox19.Text);
            year3 = Convert.ToInt32(TextBox20.Text);
            num1 = Convert.ToInt32(TextBox21.Text);
            num2 = Convert.ToInt32(TextBox22.Text);
            num3 = Convert.ToInt32(TextBox23.Text);
            start1 = Convert.ToInt32(TextBox26.Text);
            start2 = Convert.ToInt32(TextBox25.Text);
            start3 = Convert.ToInt32(TextBox24.Text);
            end1 = Convert.ToInt32(TextBox27.Text);
            end2 = Convert.ToInt32(TextBox28.Text);
            end3 = Convert.ToInt32(TextBox29.Text);
            getmoney = Convert.ToDouble(TextBox33.Text);
            numget = Convert.ToInt32(TextBox35.Text);
            yearnext = Convert.ToDouble(TextBox34.Text);
            startnext = Convert.ToInt32(TextBox36.Text);
            endnext = Convert.ToInt32(TextBox37.Text);
            lasti = Convert.ToDouble(TextBox31.Text);
            lastmoney = Convert.ToDouble(TextBox32.Text);
            inpv = Convert.ToDouble(TextBox56.Text);
            nowM = Convert.ToDouble(TextBox57.Text);

            if (TextBox122.Text == "")
            {
                y = calc.fvP3(moneyget, iper1, num1, iper2, num2, iper3, num3, getmoney, numget);
                x = calc.last(lasti, lastmoney, moneyget);
                z = x + y;
                TextBox46.Text = z + "";
                TextBox48.Text = x + "";
                x = calc.pv(payyear, yearpay);
                TextBox122.Text = x + "";
                x = calc.npv1(moneyget, inum, iper1, iper2, iper3, num1, num2, num3, yearpay, payyear, inpv, yearget, year1, year2, year3, end1, end2, end3, lasti, lastmoney, start1, start2, start3);
                TextBox47.Text = x + "";
                x = calc.nowP(payyear, yearpay, nowM);
                TextBox57.Text = x + "";
            }
            else if ((TextBox122.Text != "") && (TextBox126.Text == ""))
            {
                y = calc.fvP3(moneyget, iper1, num1, iper2, num2, iper3, num3, getmoney, numget);
                x = calc.last(lasti, lastmoney, moneyget);
                z = x + y;
                TextBox127.Text = z + "";
                TextBox128.Text = x + "";
                x = calc.pv(payyear, yearpay);
                TextBox126.Text = x + "";
                x = calc.npv1(moneyget, inum, iper1, iper2, iper3, num1, num2, num3, yearpay, payyear, inpv, yearget, year1, year2, year3, end1, end2, end3, lasti, lastmoney, start1, start2, start3);
                TextBox129.Text = x + "";
                x = calc.nowP(payyear, yearpay, nowM);
                TextBox57.Text = x + "";
            }
            else if ((TextBox126.Text != "") && (TextBox130.Text == ""))
            {
                y = calc.fvP3(moneyget, iper1, num1, iper2, num2, iper3, num3, getmoney, numget);
                x = calc.last(lasti, lastmoney, moneyget);
                z = x + y;
                TextBox131.Text = z + "";
                TextBox132.Text = x + "";
                x = calc.pv(payyear, yearpay);
                TextBox130.Text = x + "";
                x = calc.npv1(moneyget, inum, iper1, iper2, iper3, num1, num2, num3, yearpay, payyear, inpv, yearget, year1, year2, year3, end1, end2, end3, lasti, lastmoney, start1, start2, start3);
                TextBox133.Text = x + "";
                x = calc.nowP(payyear, yearpay, nowM);
                TextBox57.Text = x + "";
            }
            else if ((TextBox130.Text != "") && (TextBox134.Text == ""))
            {
                y = calc.fvP3(moneyget, iper1, num1, iper2, num2, iper3, num3, getmoney, numget);
                x = calc.last(lasti, lastmoney, moneyget);
                z = x + y;
                TextBox135.Text = z + "";
                TextBox136.Text = x + "";
                x = calc.pv(payyear, yearpay);
                TextBox134.Text = x + "";
                x = calc.npv1(moneyget, inum, iper1, iper2, iper3, num1, num2, num3, yearpay, payyear, inpv, yearget, year1, year2, year3, end1, end2, end3, lasti, lastmoney, start1, start2, start3);
                TextBox137.Text = x + "";
                x = calc.nowP(payyear, yearpay, nowM);
                TextBox57.Text = x + "";
            }
            else if ((TextBox134.Text != "") && (TextBox138.Text == ""))
            {
                y = calc.fvP3(moneyget, iper1, num1, iper2, num2, iper3, num3, getmoney, numget);
                x = calc.last(lasti, lastmoney, moneyget);
                z = x + y;
                TextBox139.Text = z + "";
                TextBox140.Text = x + "";
                x = calc.pv(payyear, yearpay);
                TextBox138.Text = x + "";
                x = calc.npv1(moneyget, inum, iper1, iper2, iper3, num1, num2, num3, yearpay, payyear, inpv, yearget, year1, year2, year3, end1, end2, end3, lasti, lastmoney, start1, start2, start3);
                TextBox141.Text = x + "";
                x = calc.nowP(payyear, yearpay, nowM);
                TextBox57.Text = x + "";
            }
        }



        protected void Button6_Click(object sender, EventArgs e)
        {
            TextBox1.Text = "";
            TextBox2.Text = "";
            TextBox3.Text = "";
            TextBox4.Text = "";
        }

        protected void Button8_Click(object sender, EventArgs e)
        {
             TextBox9.Text = "";
             TextBox10.Text = "";
        }

        protected void Button10_Click(object sender, EventArgs e)
        {
            TextBox5.Text = "";
            TextBox6.Text = "";
            TextBox7.Text = "";
            TextBox8.Text = "";
        }

        protected void Button12_Click(object sender, EventArgs e)
        {
            TextBox11.Text = "";
            TextBox12.Text = "";
            TextBox13.Text = "";
            TextBox14.Text = "";
            TextBox33.Text = "";
            TextBox34.Text = "";
            TextBox35.Text = "";
            TextBox36.Text = "";
            TextBox37.Text = "";
        }

        


    }
}
