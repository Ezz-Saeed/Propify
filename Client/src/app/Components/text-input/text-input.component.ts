import { CommonModule } from '@angular/common';
import { Component, Input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, FormsModule, NgControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  imports: [FormsModule,ReactiveFormsModule, CommonModule],
  templateUrl: './text-input.component.html',
  styleUrl: './text-input.component.css'
})
export class TextInputComponent implements ControlValueAccessor {
  @Input() label!:string;
  @Input() type = 'text';

  constructor(@Self() public ngControl:NgControl){
    this.ngControl.valueAccessor = this;
  }

  writeValue(obj: any): void {
  }
  registerOnChange(fn: any): void {
  }
  registerOnTouched(fn: any): void {
  }
  setDisabledState?(isDisabled: boolean): void {
  }

  get control():FormControl{
    return this.ngControl.control as FormControl;
  }

}
