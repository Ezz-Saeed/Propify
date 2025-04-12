import { Component, OnInit, ViewChild } from '@angular/core';
import { TabDirective, TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs';
import { PropertiesComponent } from "../owner-properties/properties.component";
import { AddPropertyComponent } from "../../Properties/add-property/add-property.component";
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-owner-dashboard',
  imports: [TabsModule, PropertiesComponent, AddPropertyComponent],
  templateUrl: './owner-dashboard.component.html',
  styleUrl: './owner-dashboard.component.css'
})
export class OwnerDashboardComponent implements OnInit {
  @ViewChild('ownerDashboard', {static:true}) ownerDashboard!:TabsetComponent;
  activeTab?:TabDirective;

  constructor(private activatedRoute:ActivatedRoute){}

  ngOnInit(): void {
    this.activatedRoute.queryParamMap.subscribe(params=>{
      params.get('tab') ?
      this.selectedTab(Number(params.get('tab'))) : this.selectedTab(0);
    })
  }

  selectedTab(id:number){
    this.ownerDashboard.tabs[id].active = true;
  }

  onTabActivated(data:TabDirective){
    this.activeTab = data;
  }
}
