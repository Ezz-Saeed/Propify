import { Routes } from '@angular/router';
import { LoginComponent } from './Components/login/login.component';
import { RegisterComponent } from './Components/register/register.component';
import { PropertyListComponent } from './Components/Properties/property-list/property-list.component';
import { AddPropertyComponent } from './Components/Properties/add-property/add-property.component';

export const routes: Routes = [
  {path:'login', component:LoginComponent},
  {path:'register', component:RegisterComponent},
  {path:'properties', component:PropertyListComponent},
  {path:'newProperty', component:AddPropertyComponent},
];
