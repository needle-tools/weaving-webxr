using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
#if UNITY_2020_2_OR_NEWER
using UnityEngine.SubsystemsImplementation;
#endif

namespace needle.weaver.webxr
{
    /// <summary>
    /// A base class for subsystems whose lifetime is managed by a <c>MonoBehaviour</c>.
    /// </summary>
    /// <typeparam name="TSubsystem">The <c>Subsystem</c> which provides this manager data.</typeparam>
    /// <typeparam name="TSubsystemDescriptor">The <c>SubsystemDescriptor</c> required to create the Subsystem.</typeparam>
    public abstract class SubsystemLifecycleManager<TSubsystem, TSubsystemDescriptor> : MonoBehaviour 
        where TSubsystem : IntegratedSubsystem<TSubsystemDescriptor>
        where TSubsystemDescriptor : ISubsystemDescriptor
    {
        /// <summary>
        /// Get the <c>TSubsystem</c> whose lifetime this component manages.
        /// </summary>
        public TSubsystem subsystem { get; private set; }

        /// <summary>
        /// The descriptor for the subsystem.
        /// </summary>
        /// <value>
        /// The descriptor for the subsystem.
        /// </value>
        public TSubsystemDescriptor descriptor => subsystem.SubsystemDescriptor;

        /// <summary>
        /// Returns the active <c>TSubsystem</c> instance if present, otherwise returns null.
        /// </summary>
        /// <returns>The active subsystem instance, or `null` if there isn't one.</returns>
        protected abstract TSubsystem GetActiveSubsystemInstance();

        /// <summary>
        /// Called by derived classes to initialize the subsystem is initialized before use
        /// </summary>
        protected void EnsureSubsystemInstanceSet()
        {
            subsystem = GetActiveSubsystemInstance();
        }

        /// <summary>
        /// Creates the <c>TSubsystem</c>.
        /// </summary>
        protected virtual void OnEnable()
        {
            EnsureSubsystemInstanceSet();

            if (subsystem != null)
            {
                OnBeforeStart();

                // The derived class may disable the
                // component if it has invalid state
                if (enabled)
                {
                    subsystem.Start();
                    OnAfterStart();
                }
            }
        }

        /// <summary>
        /// Stops the <c>TSubsystem</c>.
        /// </summary>
        protected virtual void OnDisable()
        {
            subsystem?.Stop();
        }

        /// <summary>
        /// Destroys the <c>TSubsystem</c>.
        /// </summary>
        protected virtual void OnDestroy()
        {
            subsystem = null;
        }

        /// <summary>
        /// Invoked after creating the subsystem and before calling Start on it.
        /// The <see cref="subsystem"/> is not <c>null</c>.
        /// </summary>
        protected virtual void OnBeforeStart()
        { }

        /// <summary>
        /// Invoked after calling Start on it the Subsystem.
        /// The <see cref="subsystem"/> is not <c>null</c>.
        /// </summary>
        protected virtual void OnAfterStart()
        { }
    }
}
