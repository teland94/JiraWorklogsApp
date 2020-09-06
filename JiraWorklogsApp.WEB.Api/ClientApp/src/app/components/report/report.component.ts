import {JiraConnectionsService} from '../../services/jira-connections.service';
import {Component, OnInit} from '@angular/core';
import {ReportService} from '../../services/report.service';
import {DatePipe} from '@angular/common';
import {JiraProject} from '../../models/jira-project.model';
import {GetReportListParams} from '../../models/get-report-list-params.model';
import {DateRange} from '../../models/date-range.model';
import {ToastrService} from 'ngx-toastr';
import {LocalDataSource} from 'ng2-smart-table';
import {GetAssignableUsersParams} from '../../models/get-assignable-users-params';
import {JiraUser} from '../../models/jira-user';

@Component({
  selector: "app-report",
  templateUrl: "./report.component.html",
  styleUrls: ["./report.component.css"]
})
export class ReportComponent implements OnInit {

  settings = {
    actions: false,
    hideSubHeader: true,
    columns: {
      date: {
        title: 'Date',
        filter: true,
        sort: true,
        valuePrepareFunction: date => {
          const raw = new Date(date);
          return this.datePipe.transform(raw);
        }
      },
      userName: {
        title: 'User Name'
      },
      issueKey: {
        title: 'Issue Key'
      },
      issueTitle: {
        title: 'Issue Title'
      },
      storyPointEstimate: {
        title: 'Estimate'
      },
      worklogDescription: {
        title: 'Worklog Description'
      },
      hours: {
        title: 'Hours',
        valuePrepareFunction: hours => {
          return Math.round(hours * 100) / 100;
        }
      }
    },
    attr: {
      class: 'table'
    },
    pager: {
      display: true,
      perPage: 25
    }
  };

  projects: Array<JiraProject>;
  projectUsers: Array<JiraUser>;
  reportData: LocalDataSource;
  totalHours: number;

  dateRangePickerModel: Date[];
  currentProject: JiraProject = null;
  currentUser: JiraUser = null;

  constructor(
    private reportService: ReportService,
    private jiraConnectionService: JiraConnectionsService,
    private datePipe: DatePipe,
    private toastrService: ToastrService
  ) {
    const date = new Date();
    const pastDate = new Date();
    pastDate.setDate(pastDate.getDate() - 7);
    this.dateRangePickerModel = [pastDate, date];
  }

  ngOnInit() {
    const connections = this.jiraConnectionService.getUserLocalConnections();

    this.reportService.getProjects(connections).subscribe(data => {
      if (data) {
        this.projects = data;
        this.currentProject = data[0];
        this.projectChanged();
      }
    });
  }

  projectChanged() {
    if (!this.currentProject) {
      return;
    }
    const connection = this.jiraConnectionService.getLocalConnectionById(
      this.currentProject.jiraConnection.id
    );
    this.reportService.getAssignableUsers(new GetAssignableUsersParams(
      this.currentProject.key,
      connection ? connection : this.currentProject.jiraConnection
    )).subscribe(data => {
      this.projectUsers = data;
    })
  }

  loadReportList() {
    if (!this.currentProject || !this.currentProject.jiraConnection) {
      return;
    }
    const connection = this.jiraConnectionService.getLocalConnectionById(
      this.currentProject.jiraConnection.id
    );
    this.reportService
      .getReportList(
        new GetReportListParams(
          new DateRange(
            this.dateRangePickerModel[0],
            this.dateRangePickerModel[1]
          ),
          this.currentProject.key,
          connection ? connection : this.currentProject.jiraConnection,
          this.currentUser ? this.currentUser.emailAddress : null
        )
      )
      .subscribe(data => {
        this.reportData = new LocalDataSource(data);
        this.totalHours = data.map(ri => ri.hours)
                              .reduce((a, b) => a + b, 0);
      }, error => {
        if (error.status === 400) {
          this.toastrService.error(error.error, 'Error');
        }
        console.log(error);
      });
  }

  reportToExcel() {
    if (!this.currentProject || !this.currentProject.jiraConnection) {
      return;
    }
    const connection = this.jiraConnectionService.getLocalConnectionById(
      this.currentProject.jiraConnection.id
    );

    this.reportService
      .getReportExcelFile(
        new GetReportListParams(
          new DateRange(this.dateRangePickerModel[0], this.dateRangePickerModel[1]),
          this.currentProject.key,
          connection ? connection : this.currentProject.jiraConnection,
          this.currentUser ? this.currentUser.emailAddress : null
        )
      )
      .subscribe(data => {
        const url = window.URL.createObjectURL(data);

        const downloadLink = document.createElement('a');
        downloadLink.href = url;
        downloadLink.download = `Report ${new Date().toLocaleDateString()}.xlsx`;
        downloadLink.click();

        window.URL.revokeObjectURL(url);
      }, error => {
        if (error.status === 400) {
          this.toastrService.error(error.error, 'Error');
        }
        console.log(error);
      });
  }
}
