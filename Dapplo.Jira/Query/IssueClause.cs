﻿#region Dapplo 2017-2018 - GNU Lesser General Public License

// Dapplo - building blocks for .NET applications
// Copyright (C) 2017-2018 Dapplo
// 
// For more information see: http://dapplo.net/
// Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
// This file is part of Dapplo.Jira
// 
// Dapplo.Jira is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Dapplo.Jira is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have a copy of the GNU Lesser General Public License
// along with Dapplo.Jira. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#endregion

#region Usings

#endregion

#region Usings

using System.Linq;
using Dapplo.Jira.Entities;

#endregion

namespace Dapplo.Jira.Query
{
	/// <summary>
	///     A clause for content identifying values like ancestor, content, id and parent
	/// </summary>
	public class IssueClause : IIssueClause
	{
		private readonly Clause _clause = new Clause
		{
			Field = Fields.IssueKey
		};

		private bool _negate;

		/// <inheritDoc />
		public IIssueClause Not
		{
			get
			{
				_negate = !_negate;
				return this;
			}
		}

		/// <inheritDoc />
		public IFinalClause Is(string issueKey)
		{
			_clause.Operator = Operators.EqualTo;
			_clause.Value = issueKey;
			if (_negate)
			{
				_clause.Negate();
			}
			return _clause;
		}

		/// <inheritDoc />
		public IFinalClause Is(Issue issue)
		{
			return Is(issue.Key);
		}

		/// <inheritDoc />
		public IFinalClause In(params string[] issueKeys)
		{
			_clause.Operator = Operators.In;
			_clause.Value = "(" + string.Join(", ", issueKeys) + ")";
			if (_negate)
			{
				_clause.Negate();
			}
			return _clause;
		}

		/// <inheritDoc />
		public IFinalClause In(params Issue[] issues)
		{
			return In(issues.Select(issue => issue.Key).ToArray());
		}

		/// <inheritDoc />
		public IFinalClause InIssueHistory()
		{
			_clause.Operator = Operators.In;
			_clause.Value = "issueHistory()";
			if (_negate)
			{
				_clause.Negate();
			}
			return _clause;
		}

		/// <inheritDoc />
		public IFinalClause InLinkedIssues(string issueKey, string linkType = null)
		{
			_clause.Operator = Operators.In;
			var linkTypeArgument = string.IsNullOrEmpty(linkType) ? "" : $", {linkType}";

			_clause.Value = $"linkedIssues({issueKey}{linkTypeArgument})";
			if (_negate)
			{
				_clause.Negate();
			}
			return _clause;
		}

		/// <inheritDoc />
		public IFinalClause InLinkedIssues(Issue issue, string linkType = null)
		{
			return InLinkedIssues(issue.Key, linkType);
		}

		/// <inheritDoc />
		public IFinalClause InVotedIssues()
		{
			_clause.Operator = Operators.In;
			_clause.Value = "votedIssues()";
			if (_negate)
			{
				_clause.Negate();
			}
			return _clause;
		}

		/// <inheritDoc />
		public IFinalClause InWatchedIssues()
		{
			_clause.Operator = Operators.In;
			_clause.Value = "watchedIssues()";
			if (_negate)
			{
				_clause.Negate();
			}
			return _clause;
		}
	}
}