import { Routes } from '@angular/router';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { TestErrorComponent } from './errors/test-error/test-error.component';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { authGuard } from './_guards/auth.guard';

export const routes: Routes = [
    {
        path: '',
        component: HomeComponent
    },
    {
        path: "",
        runGuardsAndResolvers: 'always',
        canActivate: [authGuard],
        children: [
            {
                path: 'members',
                component: MemberListComponent,
            },
            {
                path: 'members/:id',
                component: MemberDetailComponent,
            },
            {
                path: 'lists',
                component: ListsComponent,
            },
            {
                path: 'messages',
                component: MessagesComponent,
            },
        ]
    },
    {
        path: "errors",
        component: TestErrorComponent
    },
    {
        path: "server-error",
        component: ServerErrorComponent
    },
    {
        path: "not-found",
        component: NotFoundComponent
    },
    {
        path: '**',
        component: HomeComponent,
        pathMatch: "full"
    },
];
