/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.PowerBI.Api.V2.Models;
using Report = Microsoft.PowerBI.Common.Api.Reports.Report;
using Dashboard = Microsoft.PowerBI.Common.Api.Reports.Dashboard;
using Dataset = Microsoft.PowerBI.Common.Api.Datasets.Dataset;
using Dataflow = Microsoft.PowerBI.Common.Api.Dataflows.Dataflow;
using Workbook = Microsoft.PowerBI.Common.Api.Workbooks.Workbook;

namespace Microsoft.PowerBI.Common.Api.Workspaces
{
    public class Workspace
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool? IsReadOnly { get; set; }

        public bool? IsOnDedicatedCapacity { get; set; }

        public string CapacityId { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public string State { get; set; }

        public bool IsOrphaned
        {
            get
            {
                if (this.State == null || this.State.Equals(WorkspaceState.Deleted) || this.Type.Equals(WorkspaceType.Group) || this.Type.Equals(WorkspaceType.PersonalGroup))
                {
                    return false;
                }

                return (this.Users == null) || (!this.Users.Any(u => u.AccessRight.Equals(WorkspaceUserAccessRight.Admin.ToString())));
            }
        }

        public IEnumerable<WorkspaceUser> Users { get; set; }

        public IEnumerable<Report> Reports { get; set; }

        public IEnumerable<Dashboard> Dashboards { get; set; }

        public IEnumerable<Dataset> Datasets { get; set; }

        public IEnumerable<Dataflow> Dataflows { get; set; }

        public IEnumerable<Workbook> Workbooks { get; set; }

        public static implicit operator Workspace(Group group)
        {
            return new Workspace
            {
                Id = new Guid(group.Id),
                Name = group.Name,
                IsReadOnly = group.IsReadOnly,
                IsOnDedicatedCapacity = group.IsOnDedicatedCapacity,
                CapacityId = group.CapacityId,
                Description = group.Description,
                Type = group.Type,
                State = group.State,
                Users = group.Users?.Select(x => (WorkspaceUser)x),
                Reports = group.Reports?.Select(x => (Report)x),
                Dashboards = group.Dashboards?.Select(x => (Dashboard)x),
                Datasets = group.Datasets?.Select(x => (Dataset)x),
                Dataflows = group.Dataflows?.Select(x => (Dataflow)x),
                Workbooks = group.Workbooks?.Select(x => (Workbook)x),
            };
        }

        public static implicit operator Group(Workspace workspace)
        {
            return new Group
            {
                Id = workspace.Id.ToString(),
                Name = workspace.Name,
                IsReadOnly = workspace.IsReadOnly,
                IsOnDedicatedCapacity = workspace.IsOnDedicatedCapacity,
                CapacityId = workspace.CapacityId,
                Description = workspace.Description,
                Type = workspace.Type,
                State = workspace.State,
                Users = workspace.Users?.Select(x => (GroupUserAccessRight)x).ToList()
            };
        }
    }
}
