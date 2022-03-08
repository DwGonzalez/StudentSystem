import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminDashboardComponent } from './components/admin-dashboard/admin-dashboard.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { LoginComponent } from './components/login/login.component';
import { ProfessorDashboardComponent } from './components/professor-dashboard/professor-dashboard.component';
import { RegisterComponent } from './components/register/register.component';
import { RoomSettingsComponent } from './components/room-settings/room-settings.component';
import { SubjectSettingsComponent } from './components/subject-settings/subject-settings.component';
import { UserManagementComponent } from './components/user-management/user-management.component';
import { AuthGuardService } from './guards/auth.service';

const routes: Routes = [
  { path: 'dashboard', component: DashboardComponent, },
  { path: 'admin', component: AdminDashboardComponent },
  { path: 'professor', component: ProfessorDashboardComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent, canActivate: [AuthGuardService] },
  { path: 'users', component: UserManagementComponent, canActivate: [AuthGuardService] },

  { path: 'rooms', component: RoomSettingsComponent, canActivate: [AuthGuardService] },
  { path: 'subjects', component: SubjectSettingsComponent, canActivate: [AuthGuardService] },

  { path: '**', component: DashboardComponent, canActivate: [AuthGuardService] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }