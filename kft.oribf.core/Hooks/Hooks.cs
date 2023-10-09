namespace kft.oribf.core.Hooks;

public class Hooks
{
    public static SeinHooks Sein { get; } = new();
    public static GameHooks Game { get; } = new();
    public static SceneHooks Scene { get; } = new();
    //public static HookCollection<HookImpl> OnDeath;
}


//public delegate void HookImpl(Action super);

//public class HookCollection<TImpl> : IEnumerable<TImpl>
//{
//    private readonly List<TImpl> hookImpls = new();

//    public IEnumerator GetEnumerator() => ((IEnumerable)hookImpls).GetEnumerator();

//    IEnumerator<TImpl> IEnumerable<TImpl>.GetEnumerator() => ((IEnumerable<TImpl>)hookImpls).GetEnumerator();

//    public void Execute()
//    {
//        foreach (var impl in hookImpls)
//        {
//            impl.invoke
//        }
//    }

//    public static HookCollection<TImpl> operator +(HookCollection<TImpl> a, TImpl b)
//    {
//        a.hookImpls.Add(b);
//        return a;
//    }
//}
