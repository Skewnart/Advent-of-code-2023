﻿using static AdventOfCode.Puzzle.Day19;

namespace AdventOfCode.Puzzle
{
    public static partial class Extensions
    {
        public static bool Test(this string condition, Scrap scrap)
        {
            string op = condition.Contains("<=") ? "<=" : (condition.Contains(">=") ? ">=" : (condition.Contains("<") ? "<" : ">"));
            var parts = condition.Split(op);
            int value = scrap.Parts[parts[0]];

            return "<=".Equals(op) ? value <= int.Parse(parts[1]) : (">=".Equals(op) ? value >= int.Parse(parts[1]) : "<".Equals(op) ? value < int.Parse(parts[1]) : value > int.Parse(parts[1]));
        }
        public static List<string> Clone(this List<string> list)
        {
            var newlist = new List<string>();
            foreach (var var in list)
            {
                newlist.Add(var.ToString());
            }
            return newlist;
        }
    }

    public class Day19 : IDayPuzzle
    {
        public string ExecutePart1()
        {
            G.ParseInput();

            long sum = 0;
            foreach (Scrap scrap in G.SCRAPS)
            {
                if (G.WORKFLOWS.First(wf => "in".Equals(wf.Name)).Execute(scrap))
                    sum += scrap.Score;
            }

            return sum.ToString();
        }
        public string ExecutePart2()
        {
            G.ParseInput();

            var currentwfs = G.GetAllPossibilitiesFromWorkFlows("A");
            List<string> list;
            while ((list = currentwfs.FirstOrDefault(wf => !"in".Equals(wf.First()))) != null)
            {
                string tocheck = list.First();
                list.RemoveAt(0);
                var toaddwfs = G.GetAllPossibilitiesFromWorkFlows(tocheck);
                int index = currentwfs.IndexOf(list);
                currentwfs.RemoveAt(index);
                foreach (var toadd in toaddwfs)
                {
                    var newlist = list.Clone();
                    newlist.InsertRange(0, toadd);
                    currentwfs.Insert(index, newlist);
                }
            }

            long sum = 0;
            foreach (List<string> wf in currentwfs)
            {
                List<int> x = new List<int>();
                List<int> m = new List<int>();
                List<int> a = new List<int>();
                List<int> s = new List<int>();

                for (int i = 1; i <= 4000; i++)
                {
                    x.Add(i);
                    m.Add(i);
                    a.Add(i);
                    s.Add(i);
                }
                Scrap scrap = new Scrap(1, 1, 1, 1);
                for (int i = 1; i <= 4000; i++)
                {
                    scrap.Parts["x"] = i;
                    foreach (string condition in wf.Where(x => x.StartsWith("x")))
                    {
                        bool test = condition.Test(scrap);
                        if (!test) x.Remove(i);
                    }
                }

                scrap = new Scrap(1, 1, 1, 1);
                for (int i = 1; i <= 4000; i++)
                {
                    scrap.Parts["m"] = i;
                    foreach (string condition in wf.Where(x => x.StartsWith("m")))
                    {
                        bool test = condition.Test(scrap);
                        if (!test) m.Remove(i);
                    }
                }

                scrap = new Scrap(1, 1, 1, 1);
                for (int i = 1; i <= 4000; i++)
                {
                    scrap.Parts["a"] = i;
                    foreach (string condition in wf.Where(x => x.StartsWith("a")))
                    {
                        bool test = condition.Test(scrap);
                        if (!test) a.Remove(i);
                    }
                }

                scrap = new Scrap(1, 1, 1, 1);
                for (int i = 1; i <= 4000; i++)
                {
                    scrap.Parts["s"] = i;
                    foreach (string condition in wf.Where(x => x.StartsWith("s")))
                    {
                        bool test = condition.Test(scrap);
                        if (!test) s.Remove(i);
                    }
                }

                sum += (long)x.Count * (long)m.Count * (long)a.Count * (long)s.Count;
            }

            return sum.ToString();
        }
        public class Scrap
        {
            public Dictionary<string, int> Parts { get; set; }
            public int Score => Parts.Values.Sum();
            public Scrap(int x, int m, int a, int s)
            {
                Parts = new Dictionary<string, int>() { { "x", x }, { "m", m }, { "a", a }, { "s", s } };
            }

        }
        public class Step
        {
            public string Condition { get; set; }
            public string Acceptance { get; set; }
            public Step(string condition, string acceptance)
            {
                Condition = condition;
                Acceptance = acceptance;
            }
        }
        public class Workflow
        {
            public string Name { get; set; }
            public List<Step> Steps { get; set; }

            public Workflow(string name, List<Step> steps)
            {
                Name = name;
                Steps = steps;
            }

            public bool Execute(Scrap scrap)
            {
                foreach (Step step in Steps)
                {
                    bool execute = false;
                    if ("true".Equals(step.Condition))
                        execute = true;
                    else
                        execute = step.Condition.Test(scrap);

                    if (execute)
                    {
                        if ("A".Equals(step.Acceptance)) return true;
                        if ("R".Equals(step.Acceptance)) return false;
                        else return G.WORKFLOWS.First(wf => step.Acceptance.Equals(wf.Name)).Execute(scrap);
                    }
                }

                return false;
            }
        }
        public static class G
        {
            public static List<Scrap> SCRAPS;
            public static List<Workflow> WORKFLOWS { get; set; }
            public static List<List<string>> GetAllPossibilitiesFromWorkFlows(string _lookingfor)
            {
                var result = new List<List<string>>();
                var allresult = G.WORKFLOWS.Where(wf => wf.Steps.Any(s => _lookingfor.Equals(s.Acceptance)));

                foreach (Workflow workflow in allresult)
                {
                    foreach (Step step in workflow.Steps.Where(s => _lookingfor.Equals(s.Acceptance)))
                    {
                        List<string> listconditions = new List<string>();
                        if (!"true".Equals(step.Condition)) listconditions.Add(step.Condition);
                        for (int i = workflow.Steps.IndexOf(step) - 1; i >= 0; i--)
                        {
                            var stepint = workflow.Steps[i];
                            var op = stepint.Condition.Contains('<') ? "<" : ">";
                            var parts = stepint.Condition.Split(op);

                            string oppoop = "<".Equals(op) ? ">=" : "<=";
                            listconditions.Insert(0, $"{parts[0]}{oppoop}{parts[1]}");
                        }
                        listconditions.Insert(0, workflow.Name);
                        result.Add(listconditions);
                    }
                }

                return result;
            }
            public static void ParseInput()
            {
                var all = INPUT.Split("\r\n\r\n");
                string wfStr = all[0];
                string scStr = all[1];

                WORKFLOWS = new List<Workflow>();
                foreach (string line in wfStr.Split("\r\n"))
                {
                    string name = line.Substring(0, line.IndexOf('{'));
                    string stepsstr = line.Substring(line.IndexOf('{'));
                    stepsstr = stepsstr.Substring(1, stepsstr.Length - 2);
                    var stepsstrarr = stepsstr.Split(",").ToArray();
                    List<Step> steps = new List<Step>();
                    foreach (string stepstr in stepsstrarr)
                    {
                        var stepstrparts = stepstr.Split(":");
                        steps.Add(new Step(stepstrparts.Length == 2 ? stepstrparts[0] : "true", stepstrparts.Length == 2 ? stepstrparts[1] : stepstrparts[0]));
                    }
                    WORKFLOWS.Add(new Workflow(name, steps));
                }

                SCRAPS = new List<Scrap>();
                foreach (string scrapstr in scStr.Split("\r\n").Select(line => line.Substring(1, line.Length - 2)))
                {
                    var scrapparts = scrapstr.Split(",").Select(x => int.Parse(x.Split("=")[1])).ToArray();
                    SCRAPS.Add(new Scrap(scrapparts[0], scrapparts[1], scrapparts[2], scrapparts[3]));
                }
            }

            public static readonly string INPUTTEST =
    @"px{a<2006:qkq,m>2090:A,rfg}
pv{a>1716:R,A}
lnx{m>1548:A,A}
rfg{s<537:gd,x>2440:R,A}
qs{s>3448:A,lnx}
qkq{x<1416:A,crn}
crn{x>2662:A,R}
in{s<1351:px,qqz}
qqz{s>2770:qs,m<1801:hdj,R}
gd{a>3333:R,R}
hdj{m>838:A,pv}

{x=787,m=2655,a=1222,s=2876}
{x=1679,m=44,a=2067,s=496}
{x=2036,m=264,a=79,s=2244}
{x=2461,m=1339,a=466,s=291}
{x=2127,m=1623,a=2188,s=1013}";

            public static readonly string INPUT =
    @"jz{m<1130:R,m>1525:A,R}
cb{m<1514:R,s<791:A,a>1449:R,xmm}
cn{x<3563:R,R}
qb{m>1908:A,a>1204:xmt,x>1001:A,zgl}
dr{m<3408:A,a>1459:A,x>1500:R,R}
nfv{a<1625:bbz,A}
zzt{s>3266:R,s<2765:A,R}
tk{m<852:gkx,s<2289:fh,A}
qc{a<1680:R,R}
klm{x<1453:R,m<2557:A,vms}
zc{x>2239:sqj,a<2898:cch,s>3063:xjt,sdq}
pdn{s>626:A,m>982:A,R}
ld{s>1514:qj,a>1307:lff,jjf}
rtt{x>377:R,m<2066:R,R}
njn{a<3023:R,R}
bvp{x<299:A,R}
nlt{m>1065:bcz,m>537:kz,fdd}
mvx{m>3404:R,R}
hgv{x<842:jh,A}
slt{s>1572:A,s<1560:R,s<1564:A,A}
sc{s<212:klm,m<2020:dm,dj}
pk{a<3727:A,m>3094:A,s<2200:A,A}
tm{s>2860:R,A}
vgc{a>1682:R,s<1630:A,s>1790:R,A}
bcz{s>1401:R,R}
zzn{a>3467:tmz,x<1855:tdx,a>3236:mrd,R}
rqm{a>1802:tph,s<670:smf,a<1663:qr,bjc}
kk{s>3184:A,a<495:R,a<979:sm,A}
cf{s<694:A,a<3346:R,a<3626:A,R}
kbj{a<155:A,a>217:A,A}
pgs{x>1777:dn,a<1485:zj,nlt}
pxq{s<2821:R,R}
kb{a>279:fz,s<1340:kbj,A}
cj{s>1424:A,x<693:R,R}
tzk{s>1982:kq,a>707:R,m>3530:rnc,R}
hhs{m<823:A,m<978:A,a>784:A,R}
bv{m<3670:A,a>3167:R,A}
mzr{s<2651:A,m<1076:qc,x<1913:trj,cl}
blk{m<959:sv,ngc}
mld{x>3498:nqk,a<2675:R,x<3382:R,R}
jvz{s<1770:hh,A}
vrt{s>1447:cs,tv}
jll{a>1102:nlq,s<564:xd,blk}
ds{s>65:A,R}
hm{m>2870:vrt,x<755:fm,m<2639:dvh,kzz}
nng{m>1973:vhf,m>1754:zrg,s<1918:bkj,R}
gs{a<1718:R,a<1813:A,A}
gsg{x>2639:zv,x<1180:pn,a>791:dp,kl}
tv{a>434:R,s>589:A,R}
dxk{m<1392:A,m<1780:A,m<2119:R,A}
pcj{s<1860:rvd,m<2944:svg,ngz}
tr{s<683:A,R}
sk{a<623:kpv,m<1467:chs,s<1758:cmf,gxk}
hcn{x>1636:A,x>1527:pfp,fr}
ftz{a>1173:R,a<1144:R,A}
ngv{a>3210:A,R}
vm{m>3344:A,A}
dz{m>549:R,gp}
lc{a>3424:R,s<1558:R,A}
kd{m>524:hhs,jfk}
fks{s>906:R,m>843:R,A}
zjm{a>2413:R,a>2248:A,R}
mrd{x<2013:R,m>355:R,A}
pfc{m<2998:tn,R}
dkg{a<3538:A,m<1446:R,s<2234:R,R}
jf{s>951:A,R}
bjc{x<857:svs,a>1755:kgb,A}
ngj{a<3667:A,A}
gj{s<1385:jll,zmn}
rm{s<1141:qv,x<2075:bn,s<1260:A,rb}
xmt{a<1244:A,R}
xpt{m<2473:A,R}
tqj{x<2728:gcb,s<441:A,m<2048:R,A}
sj{x<1411:A,a<3207:R,x<1662:R,A}
dh{m>1119:rsb,a<584:xcd,kd}
blr{m>3666:hgm,s>1537:dfn,a>1715:sg,dxr}
xph{x<748:hzc,A}
kfn{x<2302:A,m>3341:A,xcp}
lv{m<2571:A,m>2747:R,a<3098:A,A}
cfc{x>2067:R,s<2880:A,R}
xp{s<652:A,a>217:R,R}
xq{s<2798:gnf,zdt}
tfm{m<3364:R,s<1765:R,s<3249:dfv,dr}
kmg{a<3601:R,s>3637:mk,A}
xbv{s<2834:A,A}
mp{x>1438:R,x>1353:A,x<1305:pjf,R}
xlg{m>1027:A,m>475:A,x>1246:A,R}
bpf{x>2416:mz,s<3723:qrr,R}
tq{s>2726:A,a>494:R,R}
gdf{s<2713:vn,x>3160:xm,a<2767:hnk,sdc}
rc{x<218:vx,m>1912:ffl,njg}
lvj{s<1150:A,s>1917:R,A}
jlj{a>2261:R,x<605:A,m>2526:R,A}
jfq{s>633:vbz,m>895:zl,ph}
vn{x>2769:A,m<3178:qt,s<2309:A,A}
dj{s>322:sj,m>3050:A,lv}
lb{x>1987:xfl,s>3653:R,drz}
hh{s<1698:R,x>2742:A,a>2883:A,A}
pmq{x>1219:A,m>2080:A,s>3483:R,R}
rjz{m<788:A,A}
gqs{m>3788:A,a>1764:A,R}
pfb{x<458:R,a<1580:A,R}
rvl{m<1166:R,R}
hks{s>3425:njn,A}
fct{m<2058:pd,a<3495:sjz,m>2897:qh,jq}
gp{a<2496:R,A}
zz{s<2447:A,s<2577:A,s>2657:R,A}
fnk{a>441:R,A}
bhc{x<898:A,A}
hf{s>3088:A,gkn}
gcb{s<619:R,s<813:R,s>941:R,A}
xh{m>2149:A,s<2018:A,m>1968:A,R}
xhx{a<3192:A,A}
pfp{m>1444:R,x<1596:A,A}
zdt{s<3552:A,a>3231:R,A}
ngc{x>2725:A,m<1805:A,m>2093:jf,nct}
smt{m<1117:R,m<1265:A,R}
qs{s<2886:xhq,a<1477:gt,lvl}
mh{s<2826:A,s<2902:R,R}
qr{m>1044:pfb,s>977:A,tbg}
rxs{s>3190:R,tj}
rdz{a>1461:cfz,m<1889:A,R}
pzl{a>2401:A,s>3119:A,R}
qml{s>1042:R,s<413:A,A}
cv{a>2676:fjd,m<1617:ch,x<1572:qcq,tr}
bn{m<3192:A,m>3633:A,m<3455:R,R}
cch{m<1030:dz,x>1961:tm,m>1755:zzt,gzx}
nx{a>3225:qm,x>3187:mld,x>2919:kss,drs}
fpr{x>1048:R,s<381:A,R}
nrd{x<1510:svv,m>2288:mbf,x<2627:zc,qrx}
mfv{s>3590:R,m>810:frv,nqt}
tj{a<2983:A,R}
trj{x>1857:A,A}
djc{m>1579:A,a<1132:R,R}
nct{m>1952:R,s>966:R,s<750:A,R}
rr{s<2718:A,R}
gk{a<3634:A,s<2660:R,a>3808:A,R}
zpc{s>949:R,R}
sdc{m<2929:vvj,x<2422:bz,s>2977:mvx,mh}
lm{a<728:A,s>2103:hzl,x<1415:A,R}
cl{x>1961:R,s>3394:R,R}
rz{s>430:R,s<350:A,x>2387:R,R}
fb{x<513:jb,vh}
lz{m>1159:A,R}
cbq{s<3772:mf,m>2520:R,cml}
zf{s<2783:cdc,s>3290:gv,cvc}
bs{x>1813:hs,qx}
nkf{x<1020:A,A}
nj{s<1723:A,A}
sqj{x<2403:rxs,kbv}
lr{x<892:R,x>979:R,A}
fh{a<1260:R,a>1658:R,m>1716:A,A}
ql{x>822:A,a<2607:R,a>2718:R,R}
vj{s>2255:zg,a>235:A,x>1528:mzv,R}
rvd{x<2434:mgm,A}
zv{a>1266:qbq,vz}
gkx{a<659:R,R}
kv{a>526:A,vb}
mnx{m<2985:R,s<3292:A,m<3437:R,R}
btx{x>1493:A,a>518:A,A}
pc{s>1804:mmd,A}
kz{x>1636:tp,s<2458:ldm,sb}
njg{s>1545:xc,x>420:R,a>1188:ljj,bvp}
tmz{a<3696:R,s>2857:A,x<1819:R,A}
nrs{x>1257:hqz,vkh}
tkk{m<1432:A,a<1514:R,R}
dfv{x<1452:R,m>3408:R,s<2343:A,A}
fpd{x<663:A,A}
qj{x>703:R,fnq}
pbp{a<2964:slg,a>3564:cmx,x<3424:xq,ngv}
vb{m<3608:A,A}
kq{m<3582:R,A}
rcf{a<1241:hb,gs}
gnp{a>3435:vp,a<3116:xrr,s<3647:tzz,cbq}
hzk{a>1167:ndj,frj}
gnf{s>2325:R,m>1553:A,x<3097:R,R}
zx{x<1638:R,a<2394:A,x>2870:R,A}
vv{s>1853:A,A}
scl{x<1836:R,m>1491:R,s>510:A,A}
qrx{m>897:pbp,m>509:sp,nx}
kgb{s<908:A,A}
vtq{s<1508:scl,m>894:kbk,A}
zrg{m>1886:A,s<2418:R,R}
pp{s<3534:R,R}
gd{a>3604:A,R}
cvc{m>806:cn,m<509:A,A}
zgq{a<134:kxr,m>3404:vj,qpp}
gq{m>1542:fpr,s<417:R,a<1224:R,vxm}
kvn{s>1252:R,a<1415:R,x<3671:R,R}
kl{x>2054:pcj,a<374:zgq,m<3243:tx,jx}
vhf{x>1552:R,x<1458:A,a>1362:R,A}
ln{a>1366:R,a<1333:A,A}
kxr{m>3326:A,a<53:rl,s<2161:A,ckt}
xjt{x<1861:md,x<2089:lb,x<2145:dvx,xr}
tf{a<3198:R,x<3136:R,m>3856:A,A}
tp{a>1646:A,m>827:R,R}
mgm{s<954:A,a<372:R,A}
fqx{x<2429:R,nv}
dm{s>306:xhx,m<1121:A,a<3274:A,A}
zg{s<2954:R,a>262:A,R}
km{s<631:A,s>814:A,m<3329:A,A}
znt{m<2868:R,a>565:cpz,a>495:A,xmr}
clp{m>661:R,m<227:A,x<634:R,A}
lf{x<913:nt,s>469:cv,sc}
dfn{a<1749:R,s>2899:R,a<1881:A,A}
nn{a<830:A,s>2704:R,R}
fs{s<3259:A,m<1958:jz,m>3070:bv,R}
ckt{x>1743:R,R}
xht{s<3170:A,x>591:R,A}
jt{m<1776:R,s<366:R,A}
jjf{s<992:A,x>579:A,m<3254:tbc,A}
cpb{a>1546:A,s>729:A,R}
dvh{x>1002:xpt,m>2476:R,bpm}
mmd{a>3211:A,s<1880:A,a<2704:R,R}
cmk{a<1735:ssl,s<2868:xh,xph}
rd{s<940:R,m<2640:R,m<3224:R,A}
lbh{x<653:hc,vs}
xc{m<1631:A,m>1750:A,x>428:A,R}
sm{x>3170:R,a>693:R,s>2638:A,A}
gxk{m<1835:nn,vc}
qmb{a<2460:R,m>1070:A,A}
chs{x<223:rkn,m<928:fcn,m<1228:vr,A}
mk{a<3805:A,a>3930:A,m>2933:A,A}
zj{a<1302:hcn,a>1409:qzq,m<1496:jrv,nng}
bkt{x<1853:R,R}
scc{x>2294:R,R}
cmf{m>1803:R,m<1580:R,m>1692:R,sr}
hts{a>1429:R,R}
lvl{s<3554:R,m>2002:A,a<1508:A,A}
txs{a>913:A,s>1636:tdd,A}
nb{m<827:R,a<1466:A,A}
xvr{a>2259:A,x<1201:A,x<1411:R,A}
msx{s<2668:R,m>3888:R,A}
xcd{m>437:A,hk}
fhm{m>1527:fp,pzm}
pxl{x<624:A,a<2360:xt,x<1210:ql,A}
nqk{s<3179:R,a<2566:R,A}
tn{a>1612:A,m<2773:A,s<2628:A,R}
fd{x<2253:tl,a<3344:hl,x<3302:R,A}
npb{x>1358:pgs,a>1533:mdp,a>1277:lbh,mrb}
xmr{a<436:R,a<461:R,a<475:A,R}
ngh{s<3462:A,s<3710:R,x<871:R,R}
kpv{a<370:A,a<502:hdx,m>1085:R,qqv}
mdp{s<1379:rqm,m<1176:snr,m>1856:cmk,fb}
bpm{s>2451:A,x<911:A,a<321:A,R}
hk{a<232:A,R}
sp{s>2768:hks,a<3323:zz,dhx}
fgg{a<3832:A,a>3936:R,R}
xrr{x<1099:bgd,m>2402:A,dxk}
vmz{s<379:fll,m>2659:km,a>2184:pdn,mt}
xxf{s<1512:btx,R}
xf{s>1643:A,s<668:R,m<396:A,R}
vp{x>1154:A,m>2384:pp,x>975:A,A}
qsf{a>3217:A,s<2804:A,R}
nrv{m>1060:mj,hgv}
mct{m>1227:A,A}
zzj{s<1511:A,a>2404:A,R}
snr{a>1680:vmf,nfv}
sq{m<2910:A,x<3333:A,R}
gt{s>3266:R,a>1453:A,A}
sv{a<728:R,R}
ccq{s<2309:A,cvx}
gr{m<712:A,A}
vk{s<318:A,m>1499:R,A}
ll{s>2321:A,R}
sql{s>368:A,s>220:R,A}
vkh{x>497:pln,sk}
slf{m>2641:R,s>154:rvl,m>1743:A,ds}
hc{x>334:drj,cqt}
csd{s<1936:rdr,nrd}
sb{m<808:R,a>1725:R,x>1493:A,A}
qx{x>1731:A,R}
tbg{a<1602:R,A}
ldm{m>870:R,m<674:R,m<749:A,R}
cm{a<3648:R,a>3781:R,R}
xmm{s<1180:R,a<1269:R,x>1873:A,R}
fjd{x<1497:cf,m<1719:R,qqg}
mzv{s>766:R,R}
cdc{a<769:A,a<1371:dx,s<2044:vgc,A}
td{s>169:A,x>3311:R,s<159:A,A}
tx{m<2676:xxf,x<1648:znt,m>2969:bs,nk}
fz{a<533:A,x>185:R,R}
zl{a>3283:bd,A}
cs{x>846:R,A}
dx{s>1904:A,a<977:A,s<1698:R,R}
zmm{s>2201:vll,A}
rt{a>2680:R,m>3090:A,A}
pv{m>3089:R,x>2756:tjc,A}
fdd{x<1586:mhn,s<1529:mbk,rqn}
drz{m<933:A,R}
ptc{m>1663:A,x>2416:A,A}
hqz{x<1692:gdz,s>2642:dh,zt}
zt{a>701:txs,vtq}
qt{m<2751:R,R}
hb{s>2490:R,a>1045:A,a<910:R,A}
pjf{a<146:A,a<256:R,a>335:R,R}
sg{x<2038:A,x<2343:A,xnl}
drs{s<2892:hr,s>3296:A,m<171:R,grl}
tdd{m<1564:R,s<2112:R,A}
pq{m>3202:kb,bq}
md{a<3610:bp,a<3784:lq,a>3863:R,lz}
gdz{a<391:mp,lm}
gkn{x>971:R,s<2386:R,A}
svs{m>1020:A,R}
jmx{a<2547:A,a>2739:A,R}
nk{m>2801:A,a>569:bkt,R}
cmx{a<3723:A,a<3867:A,x<3269:A,kvz}
glr{s>1592:hhc,s<1383:nkr,a>2851:nbc,fhm}
ch{x>1440:kjt,a<2305:xlg,x>1201:R,qmb}
ml{a>135:A,s<2160:A,m>3684:A,A}
qzq{a>1448:A,hts}
svv{a<2918:pt,x<863:rh,s>3293:gnp,fct}
fll{m<1378:A,s<245:A,R}
cr{a<3596:A,A}
bz{m>3568:R,x<2088:R,R}
hv{x<811:R,A}
drj{m<1041:xf,x<504:R,s>2235:xht,zpc}
rlh{m<3343:kmg,a>3537:bpf,m>3604:zh,dg}
kbk{m<1804:A,R}
kx{s<973:A,s>1717:A,s>1281:R,A}
qq{s<2698:R,s>3280:R,a<1743:A,R}
xcp{s>2127:A,R}
kss{m<204:A,x<3028:pxq,A}
hr{s<2329:A,R}
zgl{m<1608:R,a<1146:R,a>1182:A,A}
qqv{a<555:A,A}
hd{m<3050:ccq,m<3233:rcf,x<1827:tfm,kfn}
vs{a<1409:khx,m<1355:fl,s>2101:qs,rdz}
ljj{x>303:A,m>1612:R,A}
db{x<1033:A,x>1091:R,R}
in{a>1968:csd,hsh}
ss{s<3370:R,a<3205:R,A}
vbz{m>1302:cr,s<849:A,fks}
nbc{s<1493:fqx,s<1540:fd,m>2158:ndh,dhk}
fnq{a>1204:R,m>3329:A,A}
rst{x>3196:crr,R}
fm{a>445:A,x<587:R,m>2640:A,R}
lq{s<3441:A,x>1645:A,x>1566:A,R}
pln{m>1004:ll,zmm}
ljx{x>927:ht,x<650:hz,rxp}
khx{s<2513:R,A}
np{a>3491:R,x>2187:A,R}
kch{m<2868:A,R}
fdj{x<905:R,x>1172:R,s>311:A,R}
nqt{x<2546:R,x>2904:A,A}
dhx{x<3174:gr,cm}
jh{m>437:A,R}
fl{a>1489:A,x<1038:lr,R}
hgm{s<1614:mkk,s>3134:gqs,x>1932:R,A}
pd{x<1168:db,m<1270:R,m<1731:A,gk}
qpp{s<1857:xp,R}
fp{m>2729:zzj,A}
vr{s>2507:R,a>806:R,a>703:R,A}
slg{m>1452:A,x<3519:kbf,s>3018:smt,zjm}
qm{x>3085:A,m<258:bnz,A}
xt{x<996:R,x<1334:A,A}
gv{x>3745:A,A}
gf{a<2893:xhf,m<2135:jfq,fdx}
pb{x<1181:R,m>645:R,R}
pj{s<2532:A,R}
rfz{s<3830:R,x>2928:A,A}
xm{s<2904:qsf,A}
rl{a>31:A,s<1819:A,A}
qqg{a<3386:A,a>3729:A,A}
xnl{m<3538:R,a>1834:R,R}
lsr{x<2294:R,A}
mg{m<603:A,s>387:R,A}
kvz{x>3584:R,s<2679:A,m>1759:A,R}
bnz{x<2830:A,R}
qkm{s<3253:R,s<3426:A,m<1092:R,R}
mrb{s>2443:nrv,m<1235:pdp,x>566:chv,rc}
mb{x<3099:fgg,R}
mz{x>3338:R,s<3733:R,A}
mbf{s<3205:gdf,a<3065:fvr,rlh}
vxm{s>652:R,x<1053:R,R}
nkr{m<1579:rx,rm}
rb{m<2452:R,R}
pzm{s>1479:jmx,x>2270:A,pb}
cg{x>1929:dkg,cz}
ngz{m>3625:scc,m<3246:xl,hzr}
rh{a<3397:fs,x<361:vsk,ngj}
tl{m>1877:A,R}
bgl{a>3615:R,pgh}
rnc{a<650:A,R}
pt{m<2458:hf,pxl}
cml{a>3274:A,A}
lg{a<1305:R,s<577:sql,A}
gnx{x<2526:A,x>2614:A,a<221:R,R}
pdp{m<768:hv,a>1195:jg,bng}
hhc{x<1965:pc,m<2232:jvz,cc}
kvg{m<1512:R,a>2150:A,a<2051:R,R}
xhf{a<2418:vmz,a<2622:bxt,tqj}
bxt{x>3111:A,A}
tb{s>3069:mfv,tk}
khm{m<2716:A,m<2852:R,R}
zmn{x<3246:tb,zf}
nt{s<362:slf,rv}
jx{a<608:kv,tzk}
kjt{m>792:R,x<1666:R,R}
bq{x<243:A,lt}
cqt{x>154:vv,s>1726:pj,A}
mkk{x<1861:A,x<2359:A,A}
bkj{x<1588:A,s>1151:A,R}
vsk{x>183:R,m<2303:R,s<2705:pk,mnx}
hz{s<1723:A,m<3761:R,a>375:msx,R}
dp{m<3442:hd,a>1506:blr,hzk}
vz{s>1634:kk,sq}
mhn{x<1475:A,a>1746:R,R}
pn{a>712:ld,x<469:pq,m>3443:ljx,hm}
dxr{x>1995:lsr,a>1602:A,m<3575:R,R}
sdq{s<2453:cg,m>1050:bgl,m<538:zzn,mjt}
cc{a>3295:nj,x>2897:rt,A}
mt{x>3318:A,s>631:A,A}
ph{m>353:mg,R}
tph{x>847:R,s>873:R,R}
dhk{x<2418:gd,s<1574:lc,R}
dg{m>3485:A,bj}
thx{s<875:R,x<782:R,R}
jb{m<1559:R,qq}
mjt{a>3626:rr,R}
vll{x<911:A,a>479:A,s>3290:A,A}
xhm{m>575:A,x<203:A,A}
tjc{s>3470:R,x>3330:R,A}
chv{s>858:qb,m>1819:bhc,a<1190:cfb,gq}
dn{s<1737:cb,mzr}
hzc{x>440:R,R}
ht{a>377:nkf,A}
rx{a<2921:zx,R}
crr{m>2975:A,m<2744:A,a<1683:R,R}
rsb{a<592:A,R}
hs{m<3096:A,A}
mj{m>1844:ftz,m<1359:mct,m<1587:A,ngh}
ndh{a<3604:slt,kch}
rqn{m>216:A,s>3141:A,R}
kbf{s<3018:R,m<1258:R,R}
bd{m<1474:A,a>3714:A,A}
ffl{s>1281:R,rtt}
gzx{a<2355:kvg,R}
jg{m>1048:fpd,s<1154:rdt,A}
nv{s<1428:A,m<2015:R,A}
rkn{a>920:A,A}
bbz{m>402:A,A}
ssl{a<1655:A,m>2085:R,R}
frj{s<2075:A,R}
dqb{s<151:A,s<203:td,xn}
kzz{x<1033:A,fnk}
cfb{a<1148:djc,s>504:qzl,a>1162:fdj,R}
sjz{x<1092:flc,a>3151:A,vm}
hdx{x<297:R,x<409:R,R}
xhq{m<2010:A,s<2583:A,x<1028:A,A}
hzl{a>952:R,a>826:A,A}
lff{x>538:thx,x<217:cpb,A}
rdt{x>874:A,s<505:A,A}
rcq{x>2767:A,m<1262:rz,s<354:ptc,gnx}
hjg{x>2774:R,A}
msl{s<2766:A,x>1387:A,m<3157:R,R}
vms{a>2956:R,m>3255:R,A}
bgd{s>3713:A,a<2996:R,a>3064:A,A}
frv{x<2766:R,a<1277:R,A}
jq{s<2826:A,a<3715:A,m<2354:A,R}
rxp{a<273:ml,x<755:R,s<1934:A,tq}
xn{x<3075:R,a<276:R,m>1182:R,R}
mbk{m>274:R,x>1672:R,R}
qcq{s<807:xvr,rd}
flc{x>974:R,s>2432:R,R}
dvx{m<1246:rjz,m<1618:A,A}
xl{s>2586:A,a<327:R,x>2276:R,R}
gmn{a<1848:R,a>1896:A,x<1154:A,A}
qh{m>3621:A,x>1266:msl,A}
fcn{m>449:A,a<811:R,R}
pgh{s>2815:R,A}
cvx{m>2737:A,s<3404:A,a>1405:R,R}
xs{m<3821:R,s<3670:R,s>3817:A,A}
vx{a>1211:kx,lvj}
tdx{m>323:A,s>2691:A,A}
rdr{s>1007:glr,x<2010:lf,gf}
fr{m>950:R,s>1932:R,x<1468:A,A}
qrr{s<3379:R,R}
kbv{m<1080:A,s<2990:A,s>3453:A,R}
hzr{a>482:A,x<2422:A,A}
cfz{s>701:R,m>1918:A,x>1053:R,A}
zh{a<3366:tf,x<2572:A,xs}
bkg{x<3019:A,s>1100:kvn,x<3457:tkk,nb}
vh{m>1610:R,m<1448:R,x<842:xbv,stp}
cpz{x>1347:A,a>662:R,A}
sr{m>1651:A,R}
tbc{x<323:R,x>451:A,s>1194:A,R}
hl{m<2414:A,m<3435:A,a>3162:A,A}
vvj{s<2946:A,A}
nlq{s>915:bkg,a<1629:lg,dhs}
fvr{s>3725:ffg,pv}
fbz{x<2717:vk,x<3476:R,R}
mf{s>3701:A,R}
grl{x>2749:R,R}
ndj{a<1301:A,s>2086:A,R}
cz{a<3296:A,s>2156:R,R}
lt{x<364:A,a<350:R,s>1979:R,R}
ffg{m>3208:R,rfz}
bj{a<3365:A,R}
vc{x>324:A,x<187:A,x>276:A,R}
qzl{a<1163:A,m>1434:A,x>903:R,R}
vmf{x>760:gmn,x<457:xhm,A}
rv{a<2720:jlj,R}
jfk{m<283:R,R}
hnk{s<3018:cfc,m>3049:pzl,m<2645:R,A}
dhs{a<1766:A,m>1060:R,a<1896:A,A}
jrv{m>576:ln,m>225:R,A}
hsh{m>2359:gsg,x>2051:gj,a>1115:npb,nrs}
tzz{a>3313:pmq,s>3494:A,ss}
svg{x>2403:khm,A}
xd{a>443:fbz,s>238:rcq,dqb}
qbq{s>1390:pfc,rst}
xfl{m>1202:R,A}
stp{x>1136:R,a>1680:R,R}
bng{a<1166:cj,a>1181:qml,s<1111:R,A}
smf{m<1106:clp,jt}
qv{s<1053:R,R}
fdx{a>3550:mb,hjg}
xr{s>3541:np,a>3529:R,a<3235:R,qkm}
bp{a>3158:R,s<3564:R,m<1367:R,R}

{x=136,m=12,a=1743,s=3439}
{x=486,m=909,a=591,s=576}
{x=1475,m=1173,a=358,s=353}
{x=790,m=465,a=12,s=84}
{x=2300,m=2673,a=762,s=1829}
{x=469,m=34,a=118,s=683}
{x=794,m=106,a=2553,s=679}
{x=571,m=385,a=312,s=201}
{x=1905,m=116,a=53,s=25}
{x=629,m=191,a=67,s=918}
{x=247,m=238,a=1053,s=493}
{x=306,m=1804,a=582,s=229}
{x=1353,m=272,a=23,s=188}
{x=540,m=629,a=58,s=303}
{x=622,m=181,a=150,s=2014}
{x=283,m=2647,a=1900,s=1582}
{x=520,m=146,a=2444,s=858}
{x=645,m=406,a=24,s=4}
{x=826,m=1598,a=766,s=605}
{x=339,m=3439,a=253,s=1454}
{x=101,m=397,a=106,s=1189}
{x=646,m=1005,a=2266,s=1288}
{x=465,m=1093,a=21,s=24}
{x=3222,m=2706,a=2295,s=4}
{x=257,m=2843,a=78,s=450}
{x=54,m=2422,a=1447,s=2626}
{x=608,m=1141,a=1,s=212}
{x=48,m=862,a=3,s=421}
{x=246,m=1065,a=2259,s=56}
{x=55,m=1706,a=715,s=196}
{x=694,m=938,a=2669,s=43}
{x=84,m=1692,a=2816,s=110}
{x=1636,m=1140,a=1963,s=1801}
{x=380,m=2452,a=2373,s=758}
{x=1153,m=175,a=403,s=1087}
{x=80,m=137,a=2735,s=128}
{x=1720,m=1894,a=1103,s=150}
{x=605,m=2624,a=850,s=3381}
{x=786,m=543,a=1437,s=296}
{x=2811,m=184,a=1944,s=3256}
{x=1378,m=3598,a=2841,s=388}
{x=864,m=129,a=2249,s=1544}
{x=683,m=1157,a=73,s=2060}
{x=2365,m=899,a=2083,s=1778}
{x=1256,m=44,a=204,s=846}
{x=197,m=762,a=1004,s=201}
{x=377,m=1039,a=1875,s=354}
{x=47,m=1276,a=1290,s=1325}
{x=496,m=11,a=991,s=776}
{x=191,m=1525,a=2141,s=155}
{x=2164,m=320,a=11,s=1256}
{x=1816,m=100,a=1033,s=1568}
{x=683,m=228,a=127,s=2152}
{x=2299,m=853,a=1551,s=110}
{x=93,m=597,a=940,s=2034}
{x=324,m=12,a=557,s=802}
{x=91,m=900,a=38,s=601}
{x=1971,m=1553,a=59,s=119}
{x=44,m=1669,a=5,s=179}
{x=2879,m=575,a=1959,s=1014}
{x=29,m=32,a=1699,s=161}
{x=2861,m=2776,a=2525,s=104}
{x=2463,m=3062,a=27,s=82}
{x=2267,m=1574,a=646,s=398}
{x=452,m=837,a=730,s=2998}
{x=3549,m=126,a=2428,s=953}
{x=47,m=1816,a=372,s=679}
{x=5,m=576,a=309,s=2453}
{x=708,m=1758,a=470,s=2915}
{x=471,m=15,a=17,s=1302}
{x=1454,m=453,a=246,s=839}
{x=1419,m=2618,a=1414,s=17}
{x=247,m=506,a=13,s=1531}
{x=90,m=597,a=52,s=293}
{x=56,m=1704,a=1099,s=35}
{x=150,m=567,a=11,s=2840}
{x=251,m=1036,a=273,s=1832}
{x=1084,m=2319,a=67,s=3403}
{x=2718,m=1315,a=1882,s=563}
{x=354,m=307,a=1513,s=1350}
{x=1091,m=1441,a=860,s=1687}
{x=1667,m=572,a=651,s=377}
{x=259,m=2680,a=197,s=2609}
{x=1149,m=2014,a=916,s=1560}
{x=1789,m=2826,a=3588,s=995}
{x=1067,m=143,a=634,s=360}
{x=1631,m=30,a=727,s=930}
{x=576,m=2741,a=689,s=2065}
{x=595,m=56,a=1330,s=547}
{x=3399,m=673,a=6,s=796}
{x=999,m=1225,a=442,s=170}
{x=2226,m=73,a=2319,s=114}
{x=11,m=2439,a=25,s=468}
{x=795,m=1216,a=1778,s=543}
{x=190,m=41,a=1122,s=302}
{x=348,m=3051,a=2817,s=1098}
{x=668,m=2073,a=1477,s=93}
{x=764,m=72,a=76,s=1241}
{x=490,m=125,a=1816,s=1495}
{x=542,m=1451,a=1058,s=1201}
{x=583,m=92,a=2232,s=2122}
{x=513,m=2250,a=328,s=72}
{x=1507,m=181,a=2049,s=595}
{x=3163,m=2065,a=1061,s=15}
{x=474,m=44,a=906,s=1042}
{x=1771,m=22,a=1351,s=937}
{x=1531,m=9,a=196,s=673}
{x=935,m=1222,a=492,s=75}
{x=48,m=1196,a=2508,s=687}
{x=900,m=1795,a=249,s=518}
{x=1915,m=2848,a=334,s=1253}
{x=357,m=1425,a=2655,s=2906}
{x=459,m=2087,a=2398,s=170}
{x=3296,m=2524,a=925,s=2248}
{x=1628,m=1067,a=914,s=1968}
{x=808,m=194,a=856,s=194}
{x=164,m=1043,a=1152,s=2619}
{x=317,m=1428,a=482,s=1802}
{x=2146,m=49,a=2448,s=220}
{x=800,m=795,a=261,s=37}
{x=1418,m=1594,a=1084,s=860}
{x=739,m=167,a=778,s=772}
{x=1849,m=801,a=3420,s=968}
{x=38,m=1697,a=2372,s=601}
{x=2802,m=562,a=1807,s=2478}
{x=1679,m=585,a=273,s=1445}
{x=2088,m=2890,a=1742,s=580}
{x=680,m=41,a=443,s=139}
{x=482,m=3382,a=1255,s=1045}
{x=1391,m=594,a=1804,s=727}
{x=1943,m=10,a=1521,s=1627}
{x=700,m=277,a=589,s=645}
{x=336,m=831,a=1916,s=1159}
{x=1880,m=507,a=939,s=97}
{x=229,m=1219,a=381,s=2488}
{x=309,m=406,a=232,s=336}
{x=287,m=1067,a=208,s=1814}
{x=49,m=188,a=60,s=410}
{x=532,m=5,a=1026,s=351}
{x=1305,m=998,a=1425,s=2385}
{x=58,m=661,a=201,s=1160}
{x=2082,m=101,a=338,s=7}
{x=2116,m=935,a=1350,s=510}
{x=635,m=630,a=1972,s=580}
{x=2127,m=2729,a=793,s=314}
{x=179,m=595,a=1410,s=1556}
{x=185,m=67,a=385,s=516}
{x=397,m=682,a=165,s=325}
{x=698,m=164,a=1978,s=1274}
{x=634,m=1303,a=193,s=263}
{x=1370,m=1521,a=507,s=309}
{x=548,m=722,a=862,s=1297}
{x=231,m=1729,a=83,s=949}
{x=493,m=795,a=1180,s=4}
{x=1514,m=2217,a=1100,s=2084}
{x=1540,m=996,a=1310,s=102}
{x=1050,m=270,a=3141,s=319}
{x=1235,m=192,a=215,s=543}
{x=30,m=939,a=562,s=3}
{x=3082,m=1567,a=1772,s=3}
{x=219,m=818,a=1128,s=1021}
{x=268,m=727,a=721,s=2141}
{x=246,m=82,a=49,s=2060}
{x=592,m=1388,a=881,s=743}
{x=1633,m=539,a=200,s=1369}
{x=157,m=269,a=2536,s=286}
{x=263,m=202,a=1486,s=2792}
{x=785,m=857,a=1712,s=1807}
{x=2015,m=847,a=58,s=470}
{x=877,m=967,a=175,s=193}
{x=1347,m=855,a=2663,s=160}
{x=300,m=1676,a=631,s=787}
{x=92,m=2002,a=1668,s=789}
{x=63,m=2179,a=993,s=1453}
{x=50,m=589,a=161,s=75}
{x=1990,m=1552,a=370,s=1648}
{x=939,m=1061,a=173,s=29}
{x=1129,m=1580,a=531,s=615}
{x=1243,m=635,a=699,s=1815}
{x=1657,m=25,a=1837,s=1161}
{x=2082,m=176,a=2611,s=2753}
{x=715,m=1680,a=1041,s=804}
{x=268,m=66,a=594,s=147}
{x=932,m=233,a=251,s=75}
{x=237,m=10,a=851,s=483}
{x=329,m=1860,a=149,s=2325}
{x=1349,m=378,a=1359,s=2744}
{x=38,m=1517,a=18,s=2744}
{x=1494,m=451,a=339,s=285}
{x=824,m=319,a=127,s=475}
{x=669,m=2,a=251,s=91}
{x=755,m=2149,a=670,s=2561}
{x=711,m=1768,a=2343,s=457}
{x=900,m=1017,a=2680,s=86}
{x=679,m=1775,a=17,s=1078}
{x=250,m=1054,a=2618,s=364}
{x=2164,m=3,a=689,s=1102}
{x=310,m=1841,a=803,s=1122}
{x=1975,m=121,a=546,s=1702}
{x=612,m=1946,a=2077,s=279}";
        }
    }
}
