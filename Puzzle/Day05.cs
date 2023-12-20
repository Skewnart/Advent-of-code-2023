namespace AdventOfCode.Puzzle
{
    public class SourceDest
    {
        public long Source { get; set; }
        public long Destination { get; set; }
        public long Length { get; set; }

        public SourceDest(long destination, long source, long length)
        {
            Source = source;
            Destination = destination;
            Length = length;
        }
    }

    public static class extensions
    {
        public static long ToSoil(this long seed)
        {
            return search(Day05.seedtosoil, seed);
        }
        public static long ToFertilize(this long soil)
        {
            return search(Day05.soiltofertilize, soil);
        }
        public static long ToWater(this long fertilize)
        {
            return search(Day05.fertilizetowater, fertilize);
        }
        public static long ToLight(this long water)
        {
            return search(Day05.watertolight, water);
        }
        public static long ToTemperature(this long light)
        {
            return search(Day05.lighttotemperature, light);
        }
        public static long ToHumidity(this long temperature)
        {
            return search(Day05.temperaturetohumidity, temperature);
        }
        public static long ToLocation(this long humidity)
        {
            return search(Day05.humiditytolocation, humidity);
        }

        private static long search(SourceDest[] table, long it)
        {
            SourceDest sourcedest = table.Where(line => line.Source <= it).OrderByDescending(line => line.Source).FirstOrDefault();
            if (sourcedest != null)
            {
                if (it >= sourcedest.Source && it < sourcedest.Source + sourcedest.Length)
                {
                    return sourcedest.Destination + (it - sourcedest.Source);
                }
            }

            return it;
        }

        public static SourceDest lowestsourcewithdest(this long it, SourceDest[] table)
        {
            SourceDest sourcedest = table.Where(line => line.Destination <= it).OrderByDescending(line => line.Destination).FirstOrDefault();
            if (sourcedest != null)
            {
                if (it >= sourcedest.Destination && it < sourcedest.Destination + sourcedest.Length)
                {
                    return sourcedest;
                }
            }

            return null;
        }
    }

    internal class Day05 : IDayPuzzle
    {
        public string ExecutePart1()
        {
            long minlocation = seeds.Select(s => s.ToSoil().ToFertilize().ToWater().ToLight().ToTemperature().ToHumidity().ToLocation()).Min();

            return minlocation.ToString();
        }
        SourceDest[][] sources = { temperaturetohumidity, lighttotemperature, watertolight, fertilizetowater, soiltofertilize, seedtosoil };
        public string ExecutePart2()
        {
            foreach(var var in humiditytolocation.OrderBy(x => x.Destination))
            {
                for(long j = var.Destination; j < var.Destination + var.Length; j++)
                {
                    long delta = j;
                    var sourcedest = j.lowestsourcewithdest(humiditytolocation);
                    for(int i = 0; i < sources.Length; i++)
                    {
                        if (sourcedest == null) break;
                        long parent = sourcedest.Source + delta;
                        //long parent = sourcedest == null ? delta : sourcedest.Source + delta;

                        sourcedest = parent.lowestsourcewithdest(sources[i]);
                        //delta = sourcedest != null ? parent - sourcedest.Destination : parent;
                        if (sourcedest != null)
                            delta = parent - sourcedest.Destination;
                    }

                    if (sourcedest != null)
                    {
                        long seedfinal = delta + sourcedest.Source;

                        if (seedfinal > -1)
                        {
                            for (int i = 0; i < seeds.Length; i++)
                            {
                                if (seedfinal >= seeds[i] && seedfinal < seeds[i] + lengths[i])
                                {
                                    return j.ToString();
                                }
                            }
                        }
                    }
                }
            }

            return "";
        }

        public static long[] seeds = { 41218238, 1255413673, 944138913, 481818804, 2906248740, 3454130719, 1920342932, 2109326496, 3579244700, 4173137165 };
        public static long[] lengths = { 421491713, 350530906, 251104806, 233571979, 266447632, 50644329, 127779721, 538709762, 267233350, 60179884 };
        public static SourceDest[] seedtosoil =
        {
new SourceDest(389477588,1222450723,8619026),
new SourceDest(369327568,3429737174,12750820),
new SourceDest(8123474,1366319913,18265500),
new SourceDest(475667857,405321476,4132049),
new SourceDest(258939826,536917987,4117275),
new SourceDest(924266396,3404859218,2487795),
new SourceDest(762699703,957158780,3328016),
new SourceDest(452528837,3222194776,18266444),
new SourceDest(196573512,1924266396,17275405),
new SourceDest(33176947,990438941,616638),
new SourceDest(047092335,2681059373,3070538),
new SourceDest(39343336,1626695089,18184257),
new SourceDest(949144352,2940939059,12572612),
new SourceDest(79719551,446641973,9027601),
new SourceDest(184073848,2711764761,1249966),
new SourceDest(077797723,2605613670,2794027),
new SourceDest(074870480,4240432416,5453488),
new SourceDest(048538268,3066665187,10406822),
new SourceDest(21185913,1124514126,9793659),
new SourceDest(157547656,773812762,8427701),
new SourceDest(300112577,20266514,665536),
new SourceDest(151949413,1808537666,559824),
new SourceDest(435484067,2724264425,1704477),
new SourceDest(80487497,1308640992,5268945),
new SourceDest(644897150,26921882,11780255),
new SourceDest(516988354,996605330,12790879),
new SourceDest(241824673,858089779,1711515),
new SourceDest(932455534,2269053207,11463680),
new SourceDest(319415958,3854507632,11606810),
new SourceDest(129405360,4237225295,320712),
new SourceDest(729160001,2097020452,17203275),
new SourceDest(635193279,3557245377,29726225),
new SourceDest(795979864,578090738,10759855),
new SourceDest(901192756,3970575741,14734551),
new SourceDest(105738000,2416384374,18922929),
new SourceDest(0,685689288,8812347),
new SourceDest(903578414,0,2026651),
new SourceDest(19122510,144724435,26059704),
new SourceDest(132612481,3170733409,5146136),
new SourceDest(496835771,2741309195,19962986),
new SourceDest(306767945,1548974917,7772017),
new SourceDest(200111916,4117921253,11930404),
new SourceDest(069995565,875204932,8195384),
new SourceDest(152606490,2633553947,4750542),
new SourceDest(70778478,1814135909,10970901),
new SourceDest(384488117,1361330442,498947),
new SourceDest(696465635,2383690008,3269436)
        };

        public static SourceDest[] soiltofertilize = {
new SourceDest(796371314,958475699,9051836),
new SourceDest(004397333,4049196179,24577111),
new SourceDest(175877891,3813840430,9654415),
new SourceDest(966430612,3997904997,5129118),
new SourceDest(155151482,799623922,7931084),
new SourceDest(250168450,2358444962,1528090),
new SourceDest(265449359,3910384589,2951793),
new SourceDest(087542169,2534702057,6760931),
new SourceDest(202725381,3631683738,11382587),
new SourceDest(52357580,2833874802,4069128),
new SourceDest(452732352,2128818900,2572683),
new SourceDest(91197164,3745509611,6833081),
new SourceDest(316551254,2602311370,6053539),
new SourceDest(017721794,2764291908,6958289),
new SourceDest(98502503,445768845,35385507),
new SourceDest(367678481,1860885729,20346952),
new SourceDest(845535174,1124639771,9439851),
new SourceDest(041749195,2373725871,16097618),
new SourceDest(330424521,2874566090,75711764),
new SourceDest(478459182,127856713,31791213),
new SourceDest(234462328,1680414394,3177100),
new SourceDest(59527983,1219038283,13897452),
new SourceDest(93048868,1712185402,14870032),
new SourceDest(571148005,1406027225,27438716),
new SourceDest(939933686,2064355253,6446364),
new SourceDest(266233336,2662846763,10144514),
new SourceDest(886889681,878934768,7954093),
new SourceDest(7297932,2154545730,20389923),
new SourceDest(9283510,1358012803,4801442),
new SourceDest(272422050,3939902526,5800247),
new SourceDest(377086647,1048994066,7564570),
new SourceDest(087304688,39283510,8857320)
        };

        public static SourceDest[] fertilizetowater =
        {
new SourceDest(988818582,3038666130,30614871),
new SourceDest(927763871,3008779749,2988638),
new SourceDest(24309691,99049201,28285650),
new SourceDest(9049201,381905707,2526049),
new SourceDest(07166197,2131018623,60206835),
new SourceDest(442767659,4213146266,8182103),
new SourceDest(957650252,3344814844,48511740),
new SourceDest(907802704,2927763871,8101587),
new SourceDest(009234554,407166197,172385242),
new SourceDest(524588689,3829932251,38321401)
        };

        public static SourceDest[] watertolight =
        {
new SourceDest(071892650,2651787028,5767997),
new SourceDest(129572620,3396952543,8159315),
new SourceDest(240611714,2163493623,48829340),
new SourceDest(0,2068015044,9547857),
new SourceDest(211165770,3074252590,2944594),
new SourceDest(592854025,0,13893836),
new SourceDest(523843782,1948369545,6901024),
new SourceDest(24090948,883610805,7635349),
new SourceDest(022159128,174281796,50168465),
new SourceDest(000444441,2923208140,7144820),
new SourceDest(5478579,959964298,26809363),
new SourceDest(84655532,1228057930,23943541),
new SourceDest(410916028,2709466998,21374114),
new SourceDest(63572211,1627286224,32108332),
new SourceDest(324937342,2017379788,5063525),
new SourceDest(183141431,3068711832,554075),
new SourceDest(939436746,3478545693,24370468),
new SourceDest(624657170,2994656349,7405548),
new SourceDest(728905119,3103698534,29325400),
new SourceDest(930463154,3978112708,31685458),
new SourceDest(731792391,675966450,20764435),
new SourceDest(375572598,138938366,3534343),
new SourceDest(247317742,3967989739,1012296),
new SourceDest(257440711,3930463154,3752658),
new SourceDest(188682189,3722250378,13625515),
new SourceDest(698712653,1467493346,15979287)
        };

        public static SourceDest[] lighttotemperature =
        {
new SourceDest(148509456,1952010509,12627083),
new SourceDest(56886372,936932802,9716280),
new SourceDest(29640090,282271594,2724628),
new SourceDest(44444108,1274282332,10758431),
new SourceDest(528329058,3192525971,21147891),
new SourceDest(566760651,2178128911,79250010),
new SourceDest(78140779,1162859130,5184989),
new SourceDest(274780288,1528329058,2018700),
new SourceDest(52028426,265852816,1641877),
new SourceDest(739807973,2970629018,22189695),
new SourceDest(88336830,840381853,5610727),
new SourceDest(29990676,0,26585281),
new SourceDest(424714410,1911677980,4033252),
new SourceDest(95843492,896489131,3379659),
new SourceDest(69062248,324530949,41280440),
new SourceDest(54049175,309517876,1501307),
new SourceDest(359260758,3868594872,42637242),
new SourceDest(465046939,3404004886,10171371),
new SourceDest(75094277,737335351,10304650),
new SourceDest(0,1214709027,5957330),
new SourceDest(324866840,2078281341,9984757),
new SourceDest(961704926,1548516066,24767088),
new SourceDest(209375810,1796186950,11549103),
new SourceDest(9573305,1034095605,12876352),
new SourceDest(785633182,3505718598,36287627),
new SourceDest(68447204,930285729,664707)
        };

        public static SourceDest[] temperaturetohumidity =
        {
new SourceDest(45925588,927807414,8714016),
new SourceDest(0,398577479,15753125),
new SourceDest(936153073,3766846194,13526956),
new SourceDest(964800672,3492411188,195778),
new SourceDest(660032389,3460150664,3226052),
new SourceDest(374126579,1182630672,36480486),
new SourceDest(334938774,2586583717,13227495),
new SourceDest(729993364,4148156458,13915168),
new SourceDest(071422638,2398735028,18784868),
new SourceDest(61859499,894601505,3320590),
new SourceDest(128085880,3902115759,24604069),
new SourceDest(966758455,1609937892,32820884),
new SourceDest(33065750,1045187965,4523041),
new SourceDest(692292913,2718858671,3770045),
new SourceDest(738931445,2854070578,5114543),
new SourceDest(869145048,2758414954,9565562),
new SourceDest(46537472,670580619,1532202),
new SourceDest(468753739,2905216014,19127865),
new SourceDest(790076881,1547435538,6250235),
new SourceDest(103241907,3096494664,36365600),
new SourceDest(78296167,1014947576,3024038),
new SourceDest(467213728,3653280748,11356544),
new SourceDest(739690951,1959797890,36355095),
new SourceDest(30471457,878535490,1606601),
new SourceDest(34173831,836762826,4177266),
new SourceDest(72165118,90734051,8020020),
new SourceDest(259271327,1128614382,5401629),
new SourceDest(852579235,2323348846,7538618),
new SourceDest(08536556,272940204,12563727),
new SourceDest(927965417,4287308142,765915),
new SourceDest(935624571,1128085880,52850),
new SourceDest(52365321,194834068,7275081),
new SourceDest(95065408,685902646,15086018),
new SourceDest(48265304,170934254,2389981),
new SourceDest(75946495,556108732,11447188),
new SourceDest(57531253,0,9073405),
new SourceDest(466897907,2756559122,185583),
new SourceDest(580779174,3494368971,15891177),
new SourceDest(25116137,267584884,535532),
new SourceDest(313287617,1938146733,2165115)
        };

        public static SourceDest[] humiditytolocation =
        {
new SourceDest(297594568,1304834363,19963629),
new SourceDest(64984478,962777545,10201162),
new SourceDest(376226732,2612009119,7854287),
new SourceDest(210191679,3257561655,7332472),
new SourceDest(60734175,2732971245,425030),
new SourceDest(552752951,3643184542,12852679),
new SourceDest(654967093,1268999863,3583450),
new SourceDest(805486965,2087320949,35971482),
new SourceDest(2263011,1608745500,17119580),
new SourceDest(225512580,3861994731,6945471),
new SourceDest(240952852,431398165,6876741),
new SourceDest(695056291,298067962,7665504),
new SourceDest(309720262,500165575,3212403),
new SourceDest(768212426,260793423,3727453),
new SourceDest(58896561,532289611,26378121),
new SourceDest(967976997,1084282606,7197757),
new SourceDest(255175315,2690551992,4241925),
new SourceDest(514000396,0,2822701),
new SourceDest(0,2539746108,7226301),
new SourceDest(283516399,2447035775,9271033),
new SourceDest(233825691,3330886375,712716),
new SourceDest(14836670,2866104927,34589750),
new SourceDest(341844298,88637325,17215609),
new SourceDest(039954568,1779941306,21522074),
new SourceDest(861994731,3931449447,36351784),
new SourceDest(950982711,414403879,1699428),
new SourceDest(454769605,3219988623,3757303),
new SourceDest(690801593,3433704416,16449023),
new SourceDest(855291831,3338013536,9569088),
new SourceDest(195879484,1080226916,405569),
new SourceDest(22677774,1995162053,9215889),
new SourceDest(601505705,796070824,16670672),
new SourceDest(497230859,1504470654,10427484),
new SourceDest(199935174,380513362,3389051),
new SourceDest(492342637,28227011,6041031),
new SourceDest(066996105,2737221548,12888337),
new SourceDest(43458817,1064789172,1543774),
new SourceDest(165201791,3598194654,4498988),
new SourceDest(689265936,374723007,579035),
new SourceDest(681279745,3212002432,798619),
new SourceDest(542227407,1156260177,11273968)
        };
    }
}
