﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.Validator;

namespace MFramework.Common.Validation.Extending.Castle
{
    // Copyright 2004-2010 Castle Project - http://www.castleproject.org/
    // 
    // Licensed under the Apache License, Version 2.0 (the "License");
    // you may not use this file except in compliance with the License.
    // You may obtain a copy of the License at
    // 
    //     http://www.apache.org/licenses/LICENSE-2.0
    // 
    // Unless required by applicable law or agreed to in writing, software
    // distributed under the License is distributed on an "AS IS" BASIS,
    // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    // See the License for the specific language governing permissions and
    // limitations under the License.
        using System;
        using System.Collections;
        using System.Collections.Generic;
        using System.Globalization;
        using System.Reflection;
        using System.Resources;
        using System.Threading;
        using System.ComponentModel.DataAnnotations;

        /// <summary>
        /// <see cref="IValidatorRegistry"/> implementation that
        /// caches the reflection and custom attributes calls for better performance.
        /// </summary>
        /// <remarks>The validators set will be extracted from the provided metadata</remarks>
        public class MFrameworkCachedMetadataValidationRegistry : IValidatorRegistry
        {
            private static readonly ResourceManager defaultResourceManager;
            private readonly ResourceManager resourceManager;

            private readonly IDictionary propertiesPerType = Hashtable.Synchronized(new Hashtable());
            private readonly IDictionary attrsPerProperty = Hashtable.Synchronized(new Hashtable());
            private Type _metaDataType;

            /// <summary>
            /// Initializes the <see cref="CachedMetadataValidationRegistry"/> class.
            /// </summary>
            static MFrameworkCachedMetadataValidationRegistry()
            {
                defaultResourceManager =
                    new ResourceManager("Castle.Components.Validator.Messages",
                                        typeof(CachedMetadataValidationRegistry).Assembly);
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="CachedMetadataValidationRegistry"/> class.
            /// </summary>
            public MFrameworkCachedMetadataValidationRegistry()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="CachedMetadataValidationRegistry"/> class.
            /// </summary>
            /// <param name="resourceManager">The resource manager.</param>
            public MFrameworkCachedMetadataValidationRegistry(ResourceManager resourceManager)
            {
                this.resourceManager = resourceManager;
            }

            #region IValidatorRegistry Members

            /// <summary>
            /// Controlla se è stato definito un attributo Metadata
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            private bool HaveMetadataAttribute(Type type)
            {
                return type.GetCustomAttributes(typeof (MetadataTypeAttribute), false).Any();
            }
            /// <summary>
            /// Controlla se esiste per il tipo specificato una metadata classe associata 
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            private bool HaveMetadata(Type type)
            {
                return HaveMetadataAttribute(type);
            }


            private Type MetadataTypeOf(Type type)
            {
                return HaveMetadata(type)
                    ? ((MetadataTypeAttribute) type.GetCustomAttributes(typeof (MetadataTypeAttribute), false)[0])
                        .MetadataClassType
                    : null;
            }
            /// <summary>
            /// Gets all validators associated with a <see cref="Type"/>.
            /// <para>
            /// The validators returned are initialized.
            /// </para>
            /// </summary>
            /// <param name="validatorRunner">The validator runner.</param>
            /// <param name="targetType">Target type.</param>
            /// <param name="runWhen">Restrict the set returned to the phase specified</param>
            /// <returns>A Validator array</returns>
            public IValidator[] GetValidators(IValidatorRunner validatorRunner, Type targetType, RunWhen runWhen)
            {
                if (HaveMetadata(targetType))
                    _metaDataType = MetadataTypeOf(targetType);

//                if ((MetadataTypeAttribute)targetType.GetCustomAttributes(typeof(MetadataTypeAttribute), false).Count()==0)
//                _metaDataType = ((MetadataTypeAttribute)targetType.GetCustomAttributes(typeof(MetadataTypeAttribute), false)[0]).MetadataClassType;

                var properties = (PropertyInfo[])propertiesPerType[targetType];

                if (properties == null)
                {
                    propertiesPerType[targetType] = properties = ResolveProperties(targetType);
                }

                var list = new List<IValidator>();

                foreach (PropertyInfo prop in properties)
                {
                    list.AddRange(GetValidators(validatorRunner, targetType, prop, runWhen));
                }

                return list.ToArray();
            }

            /// <summary>
            /// Gets all validators associated with a property.
            /// <para>
            /// The validators returned are initialized.
            /// </para>
            /// </summary>
            /// <param name="validatorRunner">The validator runner.</param>
            /// <param name="targetType">Target type.</param>
            /// <param name="property">The property.</param>
            /// <param name="runWhen">Restrict the set returned to the phase specified</param>
            /// <returns>A Validator array</returns>
            public IValidator[] GetValidators(
                IValidatorRunner validatorRunner, Type targetType, PropertyInfo property,
                RunWhen runWhen)
            {
                var builders = (object[])attrsPerProperty[property];

                if (builders == null)
                {
                    
                    var metaDataTypeProperty = _metaDataType != null ?_metaDataType.GetProperty(property.Name):null;

                    if (metaDataTypeProperty == null)
                        return (IValidator[])(new ArrayList()).ToArray(typeof(IValidator));

                    builders = metaDataTypeProperty.GetCustomAttributes(typeof(IValidatorBuilder), true);

                    // Attribute order cannot be guaranted in C#
                    // this way we assure there order by Type Name
                    Array.Sort(builders, new TypeNameComparer());

                    attrsPerProperty[property] = builders;

                    foreach (IValidatorBuilder builder in builders)
                    {
                        builder.Initialize(this, property);
                    }
                }

                var validators = new List<IValidator>();

                foreach (IValidatorBuilder builder in builders)
                {
                    IValidator validator = builder.Build(validatorRunner, targetType);

                    if (!IsValidatorOnPhase(validator, runWhen)) continue;

                    validator.Initialize(this, property);
                    validators.Add(validator);
                }

                return validators.ToArray();
            }

            /// <summary>
            /// Gets the property value accessor.
            /// </summary>
            /// <param name="property">The property.</param>
            /// <returns>The property value accessor.</returns>
            public Accessor GetPropertyAccessor(PropertyInfo property)
            {
                return AccessorUtil.GetAccessor(property);
            }

            /// <summary>
            /// Gets the expression value accessor.
            /// </summary>
            /// <param name="targetType">The target type.</param>
            /// <param name="path">The expression path.</param>
            /// <returns>The expression accessor.</returns>
            public Accessor GetFieldOrPropertyAccessor(Type targetType, string path)
            {
                return AccessorUtil.GetAccessor(targetType, path);
            }

            /// <summary>
            /// Gets the string from resource by key
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns></returns>
            public string GetStringFromResource(string key)
            {
                if (resourceManager != null)
                {
                    ResourceSet resourceSet = resourceManager.GetResourceSet(Thread.CurrentThread.CurrentCulture, true, true);
                    string result = resourceSet.GetString(key);
                    if (result != null)
                        return result;
                }

                ResourceSet defaultResourceSetForCurrentThread =
                    defaultResourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true);

                return defaultResourceSetForCurrentThread.GetString(key);
            }

            #endregion

            /// <summary>
            /// Resolve properties that will be inspected for registered validators
            /// </summary>
            /// <param name="targetType">the type to examinate properties for</param>
            /// <returns>resolved properties</returns>
            protected virtual PropertyInfo[] ResolveProperties(Type targetType)
            {
                return targetType.GetProperties();
            }

            private static bool IsValidatorOnPhase(IValidator validator, RunWhen when)
            {
                if (validator.RunWhen == RunWhen.Everytime) return true;

                return ((validator.RunWhen & when) != 0);
            }
        }
        internal class TypeNameComparer : IComparer
        {
            /// <summary>
            /// Compares two objects by type name and returns a value indicating whether one is less than, equal to, or greater than the other.
            /// </summary>
            /// <param name="x">The first object to compare.</param>
            /// <param name="y">The second object to compare.</param>
            /// <returns>
            /// Value Condition Less than zero <paramref name="x"/> is less than <paramref name="y"/>. Zero <paramref name="x"/> equals <paramref name="y"/>. Greater than zero <paramref name="x"/> is greater than <paramref name="y"/>.
            /// </returns>
            /// <exception cref="T:System.ArgumentException">Neither <paramref name="x"/> nor <paramref name="y"/> implements the <see cref="T:System.IComparable"/> interface.-or- <paramref name="x"/> and <paramref name="y"/> are of different types and neither one can handle comparisons with the other. </exception>
            public int Compare(object x, object y)
            {
                return x.GetType().Name.CompareTo(y.GetType().Name);
            }
        }    
}

