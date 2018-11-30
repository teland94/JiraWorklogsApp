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

using System;

#endregion

namespace Dapplo.Jira.Query
{
	/// <summary>
	///     A clause for date time calculations
	/// </summary>
	public class DatetimeClause : IDatetimeClause, IDatetimeClauseWithoutValue
	{
		private readonly Clause _clause;

		internal DatetimeClause(Fields datetimeField)
		{
			_clause = new Clause
			{
				Field = datetimeField
			};
		}

		/// <inheritDoc />
		public IDatetimeClauseWithoutValue On
		{
			get
			{
				_clause.Operator = Operators.EqualTo;
				return this;
			}
		}

		/// <inheritDoc />
		public IDatetimeClauseWithoutValue Before
		{
			get
			{
				_clause.Operator = Operators.LessThan;
				return this;
			}
		}

		/// <inheritDoc />
		public IDatetimeClauseWithoutValue BeforeOrOn
		{
			get
			{
				_clause.Operator = Operators.LessThanEqualTo;
				return this;
			}
		}

		/// <inheritDoc />
		public IDatetimeClauseWithoutValue After
		{
			get
			{
				_clause.Operator = Operators.GreaterThan;
				return this;
			}
		}

		/// <inheritDoc />
		public IDatetimeClauseWithoutValue AfterOrOn
		{
			get
			{
				_clause.Operator = Operators.GreaterThanEqualTo;
				return this;
			}
		}

		/// <inheritDoc />
		public IFinalClause DateTime(DateTime dateTime)
		{
			if (dateTime.Minute == 0 && dateTime.Hour == 0)
			{
				_clause.Value = $"\"{dateTime:yyyy-MM-dd}\"";
			}
			else
			{
				_clause.Value = $"\"{dateTime:yyyy-MM-dd HH-mm}\"";
			}
			return _clause;
		}

		/// <inheritDoc />
		public IFinalClause EndOfDay(TimeSpan? timeSpan = null)
		{
			_clause.Value = $"endOfDay({timeSpan.TimeSpanToIncrement()})";
			return _clause;
		}

		/// <inheritDoc />
		public IFinalClause EndOfMonth(TimeSpan? timeSpan = null)
		{
			_clause.Value = $"endOfMonth({timeSpan.TimeSpanToIncrement()})";
			return _clause;
		}

		/// <inheritDoc />
		public IFinalClause EndOfWeek(TimeSpan? timeSpan = null)
		{
			_clause.Value = $"endOfWeek({timeSpan.TimeSpanToIncrement()})";
			return _clause;
		}

		/// <inheritDoc />
		public IFinalClause EndOfYear(TimeSpan? timeSpan = null)
		{
			_clause.Value = $"endOfYear({timeSpan.TimeSpanToIncrement()})";
			return _clause;
		}

		/// <inheritDoc />
		public IFinalClause StartOfDay(TimeSpan? timeSpan = null)
		{
			_clause.Value = $"startOfDay({timeSpan.TimeSpanToIncrement()})";
			return _clause;
		}

		/// <inheritDoc />
		public IFinalClause StartOfMonth(TimeSpan? timeSpan = null)
		{
			_clause.Value = $"startOfMonth({timeSpan.TimeSpanToIncrement()})";
			return _clause;
		}

		/// <inheritDoc />
		public IFinalClause StartOfWeek(TimeSpan? timeSpan = null)
		{
			_clause.Value = $"startOfWeek({timeSpan.TimeSpanToIncrement()})";
			return _clause;
		}

		/// <inheritDoc />
		public IFinalClause StartOfYear(TimeSpan? timeSpan = null)
		{
			_clause.Value = $"startOfYear({timeSpan.TimeSpanToIncrement()})";
			return _clause;
		}
	}
}