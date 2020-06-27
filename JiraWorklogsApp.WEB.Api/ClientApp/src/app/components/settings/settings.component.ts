import { JiraConnectionsService } from '../../services/jira-connections.service';
import { JiraConnection, JiraConnectionVM } from '../../models/jira-connection.model';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { CustomValidators } from "../../custom-validators";

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {

  settingsModalRef: BsModalRef;

  settingsForm: FormGroup;
  jiraConnections: Array<JiraConnection>;
  currentJiraConnection: JiraConnection;
  isEditMode: boolean;

  constructor(private jiraConnectionsService: JiraConnectionsService,
    private modalService: BsModalService,
    private toastrService: ToastrService) {
    this.settingsForm = new FormGroup({
      name: new FormControl('', Validators.required),
      instanceUrl: new FormControl('', [Validators.required, CustomValidators.url]),
      userName: new FormControl('', [Validators.required, Validators.email]),
      authToken: new FormControl('', Validators.required),
      tempoAuthToken: new FormControl(''),
      storeToken: new FormControl(false)
    });
  }

  ngOnInit() {
    this.loadData();
  }

  add(template: TemplateRef<any>) {
    this.isEditMode = false;

    this.settingsModalRef = this.modalService.show(template);
  }

  edit(template: TemplateRef<any>, jiraConnection: JiraConnection) {
    this.isEditMode = true;
    this.currentJiraConnection = jiraConnection;

    this.settingsForm.patchValue({
      name: jiraConnection.name,
      instanceUrl: jiraConnection.instanceUrl,
      userName: jiraConnection.userName,
      authToken: jiraConnection.authToken,
      tempoAuthToken: jiraConnection.tempoAuthToken,
      storeToken: !(this.jiraConnectionsService.getLocalConnectionById(jiraConnection.id)
        || !this.currentJiraConnection.authToken)
    });

    this.settingsModalRef = this.modalService.show(template);
  }

  save() {
    const connection: JiraConnectionVM = Object.assign({}, this.settingsForm.value);
    this.testConnection(connection);
    if (this.isEditMode) {
      connection.id = this.currentJiraConnection.id;
      this.jiraConnectionsService.update(connection, connection.storeToken).subscribe(() => {
        this.loadData();
      });
    } else {
      this.jiraConnectionsService.create(connection, connection.storeToken).subscribe(data => {
        this.loadData();
      });
    }
    this.settingsModalRef.hide();
    this.settingsForm.reset();
  }

  delete(jiraConnection: JiraConnection) {
    this.jiraConnectionsService.delete(jiraConnection.id).subscribe(() => {
      this.loadData();
    });
  }

  close() {
    this.settingsModalRef.hide();
    this.settingsForm.reset();
  }

  hashPassword(password: string) {
    return '*'.repeat(password.length);
  }

  private testConnection(connection: JiraConnection) {
    this.jiraConnectionsService.test(connection).subscribe(() => {
    }, error => {
      if (error.status === 400) {
        this.toastrService.error(error.error, 'Error');
      }
      console.log(error);
    });
  }

  private loadData() {
    this.jiraConnectionsService.get().subscribe(data => {
      this.jiraConnections = data;
    });
  }
}
