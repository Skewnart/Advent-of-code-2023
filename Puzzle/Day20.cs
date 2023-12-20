namespace AdventOfCode.Puzzle
{
    public class Day20 : IDayPuzzle
    {
        public class FlipFlop : Module
        {
            public bool CurrentPulse { get; set; }
            public FlipFlop(string name, string[] children) : base(name, children)
            {
                this.CurrentPulse = false;
            }
            public override List<string> Execute(string sender, bool pulse)
            {
                List<string> tosend = new List<string>();
                if (!pulse)
                {
                    this.CurrentPulse = !CurrentPulse;

                    foreach (string child in this.Children)
                        tosend.Add($"{this.Name},{child},{(this.CurrentPulse ? 1 : 0)}");
                }

                return tosend;
            }
            public override void Init() { }
        }
        public class Conjonction : Module
        {
            public Dictionary<string, bool> Inputs { get; set; }
            public Conjonction(string name, string[] children) : base(name, children) { }
            public override List<string> Execute(string sender, bool pulse)
            {
                this.Inputs[sender] = pulse;
                bool pulsesending = this.Inputs.Values.Any(input => !input);

                var tosend = new List<string>();
                foreach (string child in this.Children)
                    tosend.Add($"{this.Name},{child},{(pulsesending ? 1 : 0)}");

                return tosend;
            }

            public override void Init()
            {
                this.Inputs = new Dictionary<string, bool>();
                foreach (Module module in G.MODULES.Where(m => m.Children.Any(y => y.Equals(this.Name))))
                {
                    this.Inputs.Add(module.Name, false);
                }
            }
        }
        public class Broadcaster : Module
        {
            public Broadcaster(string name, string[] children) : base(name, children) { }
            public override List<string> Execute(string sender, bool pulse)
            {
                var tosend = new List<string>();
                foreach (string child in this.Children)
                    tosend.Add($"{this.Name},{child},{(pulse ? 1 : 0)}");

                return tosend;
            }


            public override void Init()
            {
            }
        }
        public abstract class Module
        {
            public string Name;
            public abstract void Init();
            public abstract List<string> Execute(string sender, bool pulse);

            public string[] Children;

            public Module(string name, string[] children)
            {
                this.Name = name;
                this.Children = children;
            }
        }
        public string ExecutePart1()
        {
            G.ParseInput();

            List<int[]> highlowboucle = new List<int[]>();

            for (long i = 0; i < G.ITERATION; i++)
            {
                List<string> queue = new List<string>() { "button,broadcaster,0" };
                int low = 1, high = 0;
                while (queue.Count > 0)
                {
                    string next = queue.First();
                    queue.RemoveAt(0);
                    var parts = next.Split(',');

                    Module module = G.MODULES.FirstOrDefault(m => parts[1].Equals(m.Name));
                    if (module != null)
                    {
                        List<string> newelements = module.Execute(parts[0], "1".Equals(parts[2]));
                        queue.AddRange(newelements);
                        low += newelements.Count(x => x.EndsWith("0"));
                        high += newelements.Count(x => x.EndsWith("1"));
                    }
                }

                highlowboucle.Add(new int[] { high, low });
            }

            return (highlowboucle.Sum(x => x[0]) * highlowboucle.Sum(x => x[1])).ToString();
        }
        public string ExecutePart2()
        {
            G.ParseInput();

            var gqapparitions = new Dictionary<string, long>();

            int i = 0;
            while (true)
            {
                i++;
                List<string> queue = new List<string>() { "button,broadcaster,0" };
                while (queue.Count > 0)
                {
                    string next = queue.First();
                    queue.RemoveAt(0);
                    var parts = next.Split(',');

                    Module module = G.MODULES.FirstOrDefault(m => parts[1].Equals(m.Name));
                    if (module != null)
                    {
                        List<string> newelements = module.Execute(parts[0], "1".Equals(parts[2]));
                        queue.AddRange(newelements);
                    }

                    //C'est gq dans mon cas qui fait arriver sur rx, faut peut-être voir en fonction de l'input si ça change quelque chose
                    if (parts[1] == "gq" && parts[2] == "1" && !gqapparitions.ContainsKey(parts[0]))
                    {
                        gqapparitions.Add(parts[0], i);
                        if (gqapparitions.Count == 4) return gqapparitions.Values.Skip(1).Aggregate(gqapparitions.Values.First(), G.lcm, result => result).ToString();
                    }
                }
            }
        }

        public static class G
        {
            public static int ITERATION = 1000;
            static long pgcd(long a, long b)
            {
                while (b != 0)
                {
                    long temp = b;
                    b = a % b;
                    a = temp;
                }
                return a;
            }

            public static long lcm(long a, long b)
            {
                return (a / pgcd(a, b)) * b;
            }

            public static List<Module> MODULES;
            public static void ParseInput()
            {
                MODULES = new List<Module>();

                foreach (string line in INPUT.Split("\r\n"))
                {
                    var namesplitter = line.Split("->").Select(x => x.Trim()).ToArray();
                    string name = string.Empty;
                    char type = '\0';
                    if ("broadcaster".Equals(namesplitter[0])) name = "broadcaster";
                    else
                    {
                        name = string.Join("", namesplitter[0].Skip(1));
                        type = namesplitter[0][0];
                    }
                    var children = namesplitter[1].Split(",").Select(x => x.Trim()).ToArray();

                    MODULES.Add('\0'.Equals(type) ? new Broadcaster(name, children) : ('%'.Equals(type) ? new FlipFlop(name, children) : new Conjonction(name, children)));
                }

                foreach (Module module in MODULES)
                    module.Init();
            }

            public static readonly string INPUTTEST2 =
    @"broadcaster -> a
%a -> inv, con
&inv -> b
%b -> con
&con -> output";
            public static readonly string INPUTTEST =
    @"broadcaster -> a, b, c
%a -> b
%b -> c
%c -> inv
&inv -> a";

            public static readonly string INPUT =
    @"%dj -> fj, jn
%xz -> cm
%fn -> rj
%fv -> nt, zp
%ls -> ph, cf
%rk -> zp, tp
&jn -> km, cr, vz
%nh -> ph, ls
%tx -> gb
%xg -> dv, zp
%tp -> gx
&zp -> kj, kz, gx, fv, lv, tp
&gq -> rx
%fj -> sl, jn
%cr -> vz, jn
%rt -> fn, mf
%kj -> tt
%tk -> mg, ph
%xt -> jn, gh
%qx -> bx
%lv -> sx
%nz -> dp, ph
%sx -> kj, zp
%dd -> bf
%gb -> jp
%bj -> ph, nn
%sk -> mf
%bx -> tx, mf
%mt -> xg, zp
%vz -> hf
%vx -> mf, sk
%tt -> mt, zp
%br -> jn, fk
&xj -> gq
%mg -> ph, ps
%nt -> zp, rk
&qs -> gq
%rj -> qx, mf
%bf -> vx, mf
&kz -> gq
%fk -> jn, gk
%dv -> zp
%dp -> ph
&mf -> gb, tx, xj, dd, qx, rt, fn
&ph -> nn, xz, tk, ps, qs
%ps -> xz
&km -> gq
broadcaster -> fv, cr, rt, tk
%gk -> jn, xt
%cf -> ph, nz
%tl -> jn, br
%cm -> bj, ph
%nn -> nh
%jp -> mf, dd
%gh -> jn, dj
%hf -> tl, jn
%sl -> jn
%gx -> lv";
        }
    }
}
