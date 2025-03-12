import { Component } from '@angular/core';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { PropertiesComponent } from "../owner-properties/properties.component";
import { AddPropertyComponent } from "../../Properties/add-property/add-property.component";

@Component({
  selector: 'app-owner-dashboard',
  imports: [TabsModule, PropertiesComponent, AddPropertyComponent],
  templateUrl: './owner-dashboard.component.html',
  styleUrl: './owner-dashboard.component.css'
})
export class OwnerDashboardComponent {

}
