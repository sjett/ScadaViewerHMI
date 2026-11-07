// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Common.Presentation.Localization
{
    /// <summary>
    /// 定义一个用于管理 <see cref="ManagedMarkupExtension"/> 对象的类
    /// </summary>
    /// <remarks>
    /// 此类提供了一个单点，用于更新使用此类管理的给定标记扩展的所有标记目标.   
    /// </remarks>
    public class MarkupExtensionManager
    {

        #region Member Variables

        /// <summary>
        /// List of active extensions
        /// </summary>
        private List<ManagedMarkupExtension> _extensions = new List<ManagedMarkupExtension>();

        /// <summary>
        /// The number of extensions added since the last cleanup
        /// </summary>
        private int _cleanupCount;

        /// <summary>
        /// The interval at which to cleanup and remove extensions
        /// </summary>
        private int _cleanupInterval = 40;

        #endregion

        #region Public Interface


        /// <summary>
        /// Create a new instance of the manager
        /// </summary>
        /// <param name="cleanupInterval">
        /// The interval at which to cleanup and remove extensions associated with garbage
        /// collected targets.  This specifies the number of new Markup Extensions that are
        /// created before a cleanup is triggered
        /// </param>
        public MarkupExtensionManager(int cleanupInterval)
        {
            _cleanupInterval = cleanupInterval;
        }

        /// <summary>
        /// Force the update of all active targets that use the markup extension
        /// </summary>
        public virtual void UpdateAllTargets()
        {
            // copy the list of active targets to avoid possible errors if the list
            // is changed while enumerating
            //
            List<ManagedMarkupExtension> copy = new List<ManagedMarkupExtension>(_extensions);
            foreach (ManagedMarkupExtension extension in copy)
            {
                extension.UpdateTargets();
            }
        }

        /// <summary>
        /// Return a list of the currently active extensions
        /// </summary>
        public List<ManagedMarkupExtension> ActiveExtensions
        {
            get { return _extensions; }
        }

        /// <summary>
        /// Cleanup references to extensions for targets which have been garbage collected.
        /// </summary>
        /// <remarks>
        /// This method is called periodically as new <see cref="ManagedMarkupExtension"/> objects 
        /// are registered to release <see cref="ManagedMarkupExtension"/> objects which are no longer 
        /// required (because their target has been garbage collected).  This method does
        /// not need to be called externally, however it can be useful to call it prior to calling
        /// GC.Collect to verify that objects are being garbage collected correctly.
        /// </remarks>
        public void CleanupInactiveExtensions()
        {
            List<ManagedMarkupExtension> newExtensions = new List<ManagedMarkupExtension>(_extensions.Count);
            foreach (ManagedMarkupExtension ext in _extensions)
            {
                if (ext.IsTargetAlive)
                {
                    newExtensions.Add(ext);
                }
            }
            _extensions = newExtensions;
        }

        /// <summary>
        /// Register a new extension and remove extensions which reference target objects
        /// that have been garbage collected
        /// </summary>
        /// <param name="extension">The extension to register</param>
        internal void RegisterExtension(ManagedMarkupExtension extension)
        {
            // Cleanup extensions for target objects which have been garbage collected
            // for performance only do this periodically
            //
            if (_cleanupCount > _cleanupInterval)
            {
                CleanupInactiveExtensions();
                _cleanupCount = 0;
            }
            _extensions.Add(extension);
            _cleanupCount++;
        }

        #endregion

    }
}
