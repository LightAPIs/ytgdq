module.exports = {
  types: [
    {
      types: ['feat'],
      label: '新功能',
    },
    {
      types: ['fix'],
      label: '问题修复',
    },
    {
      types: ['perf'],
      label: '性能优化',
    },
    {
      types: ['enh'],
      label: '功能增强',
    },
    {
      types: ['tweak'],
      label: '功能改进',
    },
    {
      types: ['adjust'],
      label: '功能调整',
    },
    {
      types: ['simplify'],
      label: '功能简化',
    },
    {
      types: ['deprecate'],
      label: '功能弃用',
    },
    {
      types: ['ui'],
      label: '界面优化',
    },
    {
      types: ['security'],
      label: '安全修复',
    },
  ],
  excludeTypes: ['refactor', 'test', 'docs', 'typo', 'style', 'types', 'chore', 'config', 'build', 'ci', 'revert', 'init', 'merge'],
  renderTypeSection: function (label, commits) {
    let text = `\n## ${label}\n`;

    commits.forEach(commit => {
      text += `- ${commit.subject} (${commit.sha})\n`;
    });

    return text;
  },
};
