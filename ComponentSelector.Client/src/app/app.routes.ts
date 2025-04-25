import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { CatalogComponent } from './catalog/catalog.component';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { SettingsComponent } from './auth/settings/settings.component';
import { ProfileComponent } from './auth/settings/profile/profile.component';
import { ChangePasswordComponent } from './auth/settings/change-password/change-password.component';
import { ChangeEmailComponent } from './auth/settings/change-email/change-email.component';
import { AiSelectingComponent } from './ai-selecting/ai-selecting.component';
import { authGuard } from './_guards/auth.guard';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [authGuard],
    children: [
      {
        path: 'settings',
        component: SettingsComponent,
        children: [
          { path: 'profile', component: ProfileComponent },
          { path: 'change-password', component: ChangePasswordComponent },
          { path: 'change-email', component: ChangeEmailComponent },
          { path: '', redirectTo: 'profile', pathMatch: 'full' },
        ],
      },
    ],
  },
  {
    path: 'ai-selector',
    component: AiSelectingComponent,
  },
  { path: 'catalog/search', component: CatalogComponent },
  { path: 'catalog/:category', component: CatalogComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: '**', component: HomeComponent },
];
