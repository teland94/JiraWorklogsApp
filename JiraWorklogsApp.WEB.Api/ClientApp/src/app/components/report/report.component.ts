import { JiraConnection, JiraConnectionShortInfo } from './../../models/jira-connection.model';
import { JiraConnectionsService } from './../../services/jira-connections.service';
import { Component, OnInit } from '@angular/core';
import { ReportService } from '../../services/report.service';
import { ReportItem } from '../../models/report-item.model';
import { DatePipe } from '@angular/common';
import { JiraProject } from '../../models/jira-project.model';
import { GetReportListParams } from '../../models/get-report-list-params.model';
import { DateRange } from '../../models/date-range.model';
import 'rxjs/add/operator/finally';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: "app-report",
  templateUrl: "./report.component.html",
  styleUrls: ["./report.component.css"]
})
export class ReportComponent implements OnInit {
  private isLoading: boolean;

  settings = {
    actions: false,
    columns: {
      date: {
        title: 'Date',
        filter: true,
        sort: true,
        valuePrepareFunction: date => {
          const raw = new Date(date);
          const formatted = this.datePipe.transform(raw);
          return formatted;
        }
      },
      projectName: {
        title: 'Project Name'
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
      class: 'table table-striped'
    }
  };

  projects: Array<JiraProject>;
  reportData: Array<ReportItem>;

  dateRangePickerModel: Date[];
  currentProject: JiraProject;

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

        this.loadReportList();
      }
    });
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
          connection ? connection : this.currentProject.jiraConnection
        )
      )
      .subscribe(data => {
        this.reportData = data;
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
          connection ? connection : this.currentProject.jiraConnection
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
