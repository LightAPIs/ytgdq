module.exports = {
  types: [
    {
      types: ['feat', 'feature'],
      label: '新功能',
    },
    {
      types: ['fix', 'bugfix'],
      label: '问题修复',
    },
    {
      types: ['perf'],
      label: '优化',
    },
    {
      types: ['refactor'],
      label: '重构',
    },
    {
      types: ['build', 'ci'],
      label: '构建',
    },
    {
      types: ['other'],
      label: '其他信息',
    },
  ],
  excludeTypes: ['docs', 'style', 'test', 'chore'],
  renderTypeSection: function (label, commits) {
    let text = `\n### ${label}\n`;

    commits.forEach(commit => {
      text += `- ${commit.subject} (${commit.sha})\n`;
    });

    return text;
  },
};
