using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Korn.Service 
{
    public class PacketList
    {
        public PacketList(Type[] types) : this(types.ToList()) { }
        public PacketList(string baseofName) : this(Assembly.GetCallingAssembly(), baseofName) { }
        public PacketList(Assembly assembly, string baseofName) : this(assembly.GetTypes().Where(t => t.FullName != null && t.FullName.StartsWith(baseofName)).ToArray()) { }
        public PacketList(List<Type> types) => Types = types;

        public List<Type> Types;
        public int Count => Types.Count;

        public Type this[int index] => Types[index];

        public static implicit operator PacketList(List<Type> list) => new PacketList(list);
    }
}