// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System;
using System.Reflection;
using fitnesse.mtee.model;

namespace fitnesse.mtee.engine {
    public interface RuntimeMember {
        TypedValue Invoke(object instance, object[] parameters);
        bool MatchesParameterCount(int count);
        Type GetParameterType(int index);
        Type ReturnType { get; } //todo: can probably eliminate this
    }

    class MethodMember: RuntimeMember {
        private readonly MethodInfo info;

        public MethodMember(MemberInfo memberInfo) { info = (MethodInfo) memberInfo; }

        public bool MatchesParameterCount(int count) { return info.GetParameters().Length == count; }

        public Type GetParameterType(int index) {
            return info.GetParameters()[index].ParameterType;
        }

        public TypedValue Invoke(object instance, object[] parameters) {
            Type type = info.DeclaringType;
            object result = type.InvokeMember(info.Name,
                                              BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                                              | BindingFlags.InvokeMethod | BindingFlags.Static,
                                              null, instance, parameters);

            return new TypedValue(result, info.ReturnType);
        }

        public Type ReturnType { get { return info.ReturnType; } }
    }

    class PropertyMember: RuntimeMember {
        private readonly PropertyInfo info;

        public PropertyMember(MemberInfo memberInfo) { info = (PropertyInfo) memberInfo; }

        public Type GetParameterType(int index) {
            return info.PropertyType;
        }

        public bool MatchesParameterCount(int count) { return count == 0 && info.CanRead || count == 1 && info.CanWrite; }

        public TypedValue Invoke(object instance, object[] parameters) {
            Type type = info.DeclaringType;
            object result = type.InvokeMember(info.Name,
                                              BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                                              | (parameters.Length == 0 ? BindingFlags.GetProperty : BindingFlags.SetProperty)
                                              | BindingFlags.Static,
                                              null, instance, parameters);

            return new TypedValue(result, parameters.Length == 0 ? info.PropertyType : typeof(void));
        }

        public Type ReturnType { get { return info.PropertyType; } }
    }

    class ConstructorMember: RuntimeMember {
        private readonly ConstructorInfo info;

        public ConstructorMember(MemberInfo memberInfo) { info = (ConstructorInfo) memberInfo; }

        public Type GetParameterType(int index) {
            return info.GetParameters()[index].ParameterType;
        }

        public bool MatchesParameterCount(int count) { return info.GetParameters().Length == count; }

        public TypedValue Invoke(object instance, object[] parameters) {
            Type type = info.DeclaringType;
            object result = type.InvokeMember(info.Name,
                                     BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                                     | BindingFlags.CreateInstance,
                                     null, null, parameters);
            return new TypedValue(result, type);
        }

        public Type ReturnType { get { return info.DeclaringType; } }
    }
}