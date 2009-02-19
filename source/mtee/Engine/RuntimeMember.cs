// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System;
using System.Reflection;
using fitnesse.mtee.model;

namespace fitnesse.mtee.engine {
    public abstract class RuntimeMember {
        protected object instance;
        protected RuntimeMember(object instance) { this.instance = instance; } 

        public abstract TypedValue Invoke(object[] parameters);
        public abstract bool MatchesParameterCount(int count);
        public abstract Type GetParameterType(int index);
        public abstract Type ReturnType { get; }

    }

    class MethodMember: RuntimeMember {
        private readonly MethodInfo info;

        public MethodMember(MemberInfo memberInfo, object instance): base(instance) { info = (MethodInfo) memberInfo; }

        public override bool MatchesParameterCount(int count) { return info.GetParameters().Length == count; }

        public override Type GetParameterType(int index) {
            return info.GetParameters()[index].ParameterType;
        }

        public override TypedValue Invoke(object[] parameters) {
            Type type = info.DeclaringType;
            object result = type.InvokeMember(info.Name,
                                              BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                                              | BindingFlags.InvokeMethod | BindingFlags.Static,
                                              null, instance, parameters);

            return new TypedValue(result, info.ReturnType);
        }

        public override Type ReturnType { get { return info.ReturnType; } }
    }

    class FieldMember: RuntimeMember {
        private readonly FieldInfo info;

        public FieldMember(MemberInfo memberInfo, object instance): base(instance) { info = (FieldInfo) memberInfo; }

        public override Type GetParameterType(int index) {
            return info.FieldType;
        }

        public override bool MatchesParameterCount(int count) { return count == 0 || count == 1; }

        public override TypedValue Invoke(object[] parameters) {
            Type type = info.DeclaringType;
            object result = type.InvokeMember(info.Name,
                                              BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                                              | (parameters.Length == 0 ? BindingFlags.GetField : BindingFlags.SetField)
                                              | BindingFlags.Static,
                                              null, instance, parameters);

            return new TypedValue(result, parameters.Length == 0 ? info.FieldType : typeof(void));
        }

        public override Type ReturnType { get { return info.FieldType; } }
    }

    class PropertyMember: RuntimeMember {
        private readonly PropertyInfo info;

        public PropertyMember(MemberInfo memberInfo, object instance): base(instance) { info = (PropertyInfo) memberInfo; }

        public override Type GetParameterType(int index) {
            return info.PropertyType;
        }

        public override bool MatchesParameterCount(int count) { return count == 0 && info.CanRead || count == 1 && info.CanWrite; }

        public override TypedValue Invoke(object[] parameters) {
            Type type = info.DeclaringType;
            object result = type.InvokeMember(info.Name,
                                              BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                                              | (parameters.Length == 0 ? BindingFlags.GetProperty : BindingFlags.SetProperty)
                                              | BindingFlags.Static,
                                              null, instance, parameters);

            return new TypedValue(result, parameters.Length == 0 ? info.PropertyType : typeof(void));
        }

        public override Type ReturnType { get { return info.PropertyType; } }
    }

    class ConstructorMember: RuntimeMember {
        private readonly ConstructorInfo info;

        public ConstructorMember(MemberInfo memberInfo, object instance): base(instance) { info = (ConstructorInfo) memberInfo; }

        public override Type GetParameterType(int index) {
            return info.GetParameters()[index].ParameterType;
        }

        public override bool MatchesParameterCount(int count) { return info.GetParameters().Length == count; }

        public override TypedValue Invoke(object[] parameters) {
            Type type = info.DeclaringType;
            object result = type.InvokeMember(info.Name,
                                     BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                                     | BindingFlags.CreateInstance,
                                     null, null, parameters);
            return new TypedValue(result, type);
        }

        public override Type ReturnType { get { return info.DeclaringType; } }
    }
}