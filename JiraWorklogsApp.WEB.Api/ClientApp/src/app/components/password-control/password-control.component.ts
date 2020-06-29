import {Component, ElementRef, forwardRef, Input, ViewChild} from '@angular/core';
import {ControlValueAccessor, NG_VALUE_ACCESSOR} from "@angular/forms";

@Component({
  selector: 'app-password-control',
  templateUrl: './password-control.component.html',
  styleUrls: ['./password-control.component.css'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => PasswordControlComponent),
    multi: true
  }]
})
export class PasswordControlComponent implements ControlValueAccessor {

  private _value: string;

  showPass: boolean;
  @ViewChild('pwd') pwdField: ElementRef;

  get value() {
    return this._value;
  }

  @Input()
  set value(val) {
    this._value = val;
    this.onChange(this._value);
  }

  constructor() { }

  onChange(_: any) {}

  focusPwd() {
    const pwdFld = this.pwdField.nativeElement;
    const pwdLength = pwdFld.value.length * 2;
    pwdFld.focus();
    setTimeout(() => {
      pwdFld.setSelectionRange(pwdLength, pwdLength);
    }, 0);
  }

  writeValue(value: any) {
    this.value = value;
  }

  registerOnChange(fn) {
    this.onChange = fn;
  }

  registerOnTouched() {}
}
