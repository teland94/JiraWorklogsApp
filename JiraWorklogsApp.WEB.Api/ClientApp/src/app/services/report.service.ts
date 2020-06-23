import { JiraConnectionShortInfo } from './../models/jira-connection.model';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReportItem } from '../models/report-item.model';
import { JiraProject } from '../models/jira-project.model';
import { GetReportListParams } from '../models/get-report-list-params.model';
import { JiraUser } from '../models/jira-user';
import {GetAssignableUsersParams} from '../models/get-assignable-users-params';

@Injectable({
  providedIn: 'root'
})
export class ReportService {

  private apiUrl = 'api/Report';

  constructor(private httpClient: HttpClient) { }

  getProjects(jiraConnections: Array<JiraConnectionShortInfo>) {
    return this.httpClient.post<JiraProject[]>(this.apiUrl + '/GetProjects', jiraConnections);
  }

  getAssignableUsers(params: GetAssignableUsersParams) {
    return this.httpClient.post<JiraUser[]>(this.apiUrl + '/GetAssignableUsers', params);
  }

  getReportList(params: GetReportListParams) {
    return this.httpClient.post<ReportItem[]>(this.apiUrl + '/GetReportList', params);
  }

  getReportExcelFile(params: GetReportListParams) {
    return this.httpClient.post(this.apiUrl + '/GetReportExcelFile', params, { responseType: 'blob' as 'json' });
  }
}
