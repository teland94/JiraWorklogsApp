import { Component, Input, OnInit } from '@angular/core';
import * as XLSX from 'xlsx';
import { BsModalRef } from 'ngx-bootstrap/modal';

type AOA = any[][];

@Component({
  selector: 'app-sheet',
  templateUrl: './sheet.component.html',
  styleUrls: ['./sheet.component.css']
})
export class SheetComponent implements OnInit {

  bsModalRef: BsModalRef;

  data: AOA = [[1, 2], [3, 4]];

  @Input()
  set doc(blob: Blob) {
    const reader: FileReader = new FileReader();
    reader.onload = (e: any) => {
      const bStr: string = e.target.result;
      const wb: XLSX.WorkBook = XLSX.read(bStr, { type: 'binary' });

      const wsName: string = wb.SheetNames[0];
      const ws: XLSX.WorkSheet = wb.Sheets[wsName];

      this.data = <AOA>(XLSX.utils.sheet_to_json(ws, { header: 1, raw: false }));
      console.log(this.data);
    };
    reader.readAsBinaryString(blob);
  }

  constructor(private modalRef: BsModalRef) {
    this.bsModalRef = modalRef;
  }

  ngOnInit() {
  }
}
