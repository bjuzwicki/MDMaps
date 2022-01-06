package crc64370d450ea9a813ab;


public class MainMapRenderer
	extends crc646e4e3ae19170bac3.MapRenderer
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MDMaps.Droid.MainMapRenderer, MDMaps.Android", MainMapRenderer.class, __md_methods);
	}


	public MainMapRenderer (android.content.Context p0)
	{
		super (p0);
		if (getClass () == MainMapRenderer.class)
			mono.android.TypeManager.Activate ("MDMaps.Droid.MainMapRenderer, MDMaps.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public MainMapRenderer (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == MainMapRenderer.class)
			mono.android.TypeManager.Activate ("MDMaps.Droid.MainMapRenderer, MDMaps.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public MainMapRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == MainMapRenderer.class)
			mono.android.TypeManager.Activate ("MDMaps.Droid.MainMapRenderer, MDMaps.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
