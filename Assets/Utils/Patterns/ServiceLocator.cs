// https://erdiizgi.com/avoiding-singletons-in-unity/

using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    static readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

    public static void Register<T>( object serviceInstance )
    {
        services[ typeof( T ) ] = serviceInstance;
    }

    public static T Resolve<T>()
    {
        return (T)services[ typeof( T ) ];
    }

    public static void Reset()
    {
        services.Clear();
    }
}