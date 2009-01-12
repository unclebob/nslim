// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System;
using System.Reflection;

namespace fitnesse.mtee.engine {
    public interface RuntimeMember {
        object Invoke(object instance, object[] parameters);
        int ParameterCount {get;}
        Type GetParameterType(int index);
        Type ReturnType { get; }
    }

    class MethodMember: RuntimeMember {
        private readonly MethodInfo info;

        public MethodMember(MemberInfo memberInfo) { info = (MethodInfo) memberInfo; }

        public Type GetParameterType(int index) {
            return info.GetParameters()[index].ParameterType;
        }

        public int ParameterCount { get { return info.GetParameters().Length; } }

        public object Invoke(object instance, object[] parameters) {
            Type type = info.DeclaringType;
            object result = type.InvokeMember(info.Name,
                                              BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                                              | BindingFlags.InvokeMethod | BindingFlags.Static,
                                              null, instance, parameters);

            return info.ReturnType == typeof (void) ? typeof (void) : result;
        }

        public Type ReturnType { get { return info.ReturnType; } }
    }

    class ConstructorMember: RuntimeMember {
        private readonly ConstructorInfo info;

        public ConstructorMember(MemberInfo memberInfo) { info = (ConstructorInfo) memberInfo; }

        public Type GetParameterType(int index) {
            return info.GetParameters()[index].ParameterType;
        }

        public int ParameterCount { get { return info.GetParameters().Length; } }

        public object Invoke(object instance, object[] parameters) {
            Type type = info.DeclaringType;
            return type.InvokeMember(info.Name,
                                     BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                                     | BindingFlags.CreateInstance,
                                     null, null, parameters);
        }

        public Type ReturnType { get { return info.DeclaringType; } }
    }
}