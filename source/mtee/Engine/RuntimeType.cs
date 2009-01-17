// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System;
using System.Reflection;
using fitnesse.mtee.model;

namespace fitnesse.mtee.engine {
    public class RuntimeType {

        public Type Type { get; private set; }

        public RuntimeType(Type type) {
            Type = type;
        }

        public RuntimeMember FindInstance(string memberName, int parameterCount) {
            return Find(memberName, parameterCount, BindingFlags.Instance, null);
        }

        public RuntimeMember FindStatic(string memberName, Type[] parameterTypes) {
            return Find(memberName, parameterTypes.Length, BindingFlags.Static, parameterTypes);
        }

        public RuntimeMember GetInstance(string memberName, int parameterCount) {
            RuntimeMember runtimeMember = FindInstance(memberName, parameterCount);
            if (runtimeMember == null) throw new ArgumentException(string.Format("Member '{0}' not found for type '{1}'", memberName, Type.FullName));
            return runtimeMember;
        }

        public RuntimeMember GetConstructor(int parameterCount) {
            RuntimeMember runtimeMember = FindInstance(".ctor", parameterCount);
            if (runtimeMember == null) throw new ArgumentException(string.Format("Constructor not found for type '{0}'", Type.FullName));
            return runtimeMember;
        }

        public object CreateInstance() {
            return Type.Assembly.CreateInstance(Type.FullName);
        }

        private RuntimeMember Find(string memberName, int parameterCount, BindingFlags bindingFlags, Type[] parameterTypes) {
            var memberMatcher = new IdentifierName(memberName);
            foreach (MemberInfo memberInfo in Type.GetMembers(bindingFlags | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy)) {
                if (!memberMatcher.Matches(memberInfo.Name.Replace("_", string.Empty))) continue;
                RuntimeMember runtimeMember = MakeMember(memberInfo);
                if (Matches(runtimeMember, parameterCount, parameterTypes)) return runtimeMember;
            }
            return null;
        }

        private static bool Matches(RuntimeMember runtimeMember, int parameterCount, Type[] parameterTypes) {
            if (runtimeMember.ParameterCount != parameterCount) return false;
            if (parameterTypes == null) return true;
            for (int i = 0; i < parameterCount; i++) {
                if (runtimeMember.GetParameterType(i) != parameterTypes[i]) return false;
            }
            return true;
        }


        private static RuntimeMember MakeMember(MemberInfo memberInfo) { //todo: add other types
            switch (memberInfo.MemberType) {
                case MemberTypes.Method:
                    return new MethodMember(memberInfo);
                case MemberTypes.Constructor:
                    return new ConstructorMember(memberInfo);
                default:
                    throw new NotImplementedException(string.Format("Member type {0} not supported", memberInfo.MemberType));
            }
        }
    }
}
