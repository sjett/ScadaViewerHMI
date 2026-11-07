// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Resources;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.Infrastructure.Presentation.Services
{
    /// <summary>
    /// 页面服务基类
    /// </summary>
    public abstract class BasePageService : IPageService
    {
        private IProjectPageViewModel? page;
        /// <summary>
        /// 页面
        /// </summary>
        public IProjectPageViewModel Page
        {
            get
            {
                if (page == null)
                {
                    throw new InvalidOperationException(ErrorTexts.ServiceMustBeAttachedToPage);
                }
                return page;
            }
            set
            {
                page = value;
            }
        }
        /// <summary>
        /// 附加到页面
        /// </summary>
        /// <param name="page"></param>
        public virtual void AttachToPage(IProjectPageViewModel page)
        {
            this.page = page;
        }
    }
}
