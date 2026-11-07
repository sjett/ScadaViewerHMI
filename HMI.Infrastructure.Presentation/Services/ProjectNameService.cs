// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.ViewModels;
using System.Text.RegularExpressions;

namespace HMI.Infrastructure.Presentation.Services
{

    /// <summary>
    /// 项目名称服务实现
    /// </summary>
    public class ProjectNameService : IProjectNameService
    {
        private const string UniqueNameFormat = "{0}_{1}";
        private static readonly Regex ValidNameRegex = new Regex("([a-z]){1}([a-z0-9_])*", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private static readonly Regex NumericSuffixRegex = new Regex("_\\d+$", RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private readonly IDictionary<string, IExplorerNodeViewModel> _nodesByName = new Dictionary<string, IExplorerNodeViewModel>();
        /// <summary>
        /// 名称是否已被使用
        /// </summary>
        /// <param name="name"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool IsNameInUse(string name, IExplorerNodeViewModel node)
        {
            IExplorerNodeViewModel explorerNodeViewModel;
            this._nodesByName.TryGetValue(name, out explorerNodeViewModel);
            return explorerNodeViewModel != null && node != explorerNodeViewModel;
        }
        /// <summary>
        /// 重置名称数据库
        /// </summary>
        public void ResetNameDatabase()
        {
            this._nodesByName.Clear();
        }
        /// <summary>
        /// 注册并获取唯一名称
        /// </summary>
        /// <param name="explorerNodeViewModel"></param>
        /// <param name="proposedName"></param>
        /// <returns></returns>
        public string RegisterAndGetUniqueName(IExplorerNodeViewModel explorerNodeViewModel, string proposedName)
        {
            string uniqueName = this.GetUniqueName(explorerNodeViewModel, proposedName);
            this._nodesByName[uniqueName] = explorerNodeViewModel;
            return uniqueName;
        }
        /// <summary>
        /// 移除名称
        /// </summary>
        /// <param name="removedItems"></param>
        public void RemoveNames(IEnumerable<IExplorerNodeViewModel> removedItems)
        {
            foreach (IExplorerNodeViewModel current in removedItems)
            {
                this._nodesByName.Remove(current.ID);
            }
        }
        /// <summary>
        /// 名称是否合法
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsValidName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
            Match match = ValidNameRegex.Match(name);
            return match.Success && match.Groups[0].Length == name.Length;
        }
        /// <summary>
        /// 获取唯一名称
        /// </summary>
        /// <param name="node"></param>
        /// <param name="proposedName"></param>
        /// <returns></returns>
        private string GetUniqueName(IExplorerNodeViewModel node, string proposedName)
        {
            if (!this.IsNameInUse(proposedName, node))
            {
                return proposedName;//名称未被使用，直接返回
            }
            //去掉后缀数字部分
            string nameTrimEndNum = proposedName;
            Match match = NumericSuffixRegex.Match(proposedName);//"_数字"后缀匹配
            if (match.Success)
            {
                nameTrimEndNum = proposedName.Substring(0, match.Index);//去掉后缀的名称部分
            }
            //尝试生成唯一名称
            string uniqueName = nameTrimEndNum;
            int suffixNumber = 1;
            if (nameTrimEndNum != proposedName)//有后缀数字
            {
                uniqueName = string.Format(UniqueNameFormat, nameTrimEndNum, suffixNumber++);//尝试第一个数字
            }
            while (this.IsNameInUse(uniqueName, node))
            {
                uniqueName = string.Format(UniqueNameFormat, nameTrimEndNum, suffixNumber++);//尝试下一个数字
            }
            return uniqueName;//返回唯一名称
        }
    }
}
