import { Component, inject, OnInit, signal } from '@angular/core';
import { BuildsService } from '../../../_services/builds.service';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ComputerComponent } from '../../../_models/computerComponent';
import autoTable from 'jspdf-autotable';
import jsPDF from 'jspdf';
import { UserBuild } from '../../../_models/build/userBuild';
import '../../../../assets/fonts/Roboto-Variable-normal';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-my-builds',
  standalone: true,
  imports: [CommonModule, PaginationModule, TranslateModule ,FormsModule],
  templateUrl: './my-builds.component.html',
  styleUrl: './my-builds.component.css',
})
export class MyBuildsComponent implements OnInit {
  private toastr = inject(ToastrService);
  private router = inject(Router);
  buildsService = inject(BuildsService);
  loading = signal(false);
  currentPage = 0;

  ngOnInit(): void {
    this.currentPage = this.buildsService.buildQueryParams().pageNumber;
    if (!this.buildsService.builds()) {
      this.setBuilds();
    }
  }
  deleteBuild(id: number) {
    this.buildsService.deleteBuild(id).subscribe({
      next: () => {
        this.loading.set(true);
        this.buildsService.setUserBuilds().subscribe({
          next: (response) => this.loading.set(false),
        });
        this.toastr.success('Successful deleting');
      },
    });
  }
  pageChanged(event: any) {
    if (this.currentPage !== event.page) {
      this.currentPage = event.page;

      this.buildsService.buildQueryParams().pageNumber = event.page;
      this.setBuilds();
    }
  }
  setBuilds() {
    this.loading.set(true);
    this.buildsService.setUserBuilds().subscribe({
      next: (response) => this.loading.set(false),
    });
  }
  exportBuildToPDF(build: UserBuild): void {
    const doc = new jsPDF();

    doc.setFont('Roboto-Variable');

    doc.setFontSize(18);
    doc.text(`PC Build #${build.id}`, 14, 20);

    doc.setFontSize(12);
    doc.text(`User ID: ${build.userId}`, 14, 28);
    doc.text(`Total Price: ${build.totalPrice} грн`, 14, 35);

    const rows: any[] = [
      ...this.buildComponentRowWithLink('CPU', build.cpu),
      ...this.buildComponentRowWithLink('Motherboard', build.motherboard),
      ...this.buildComponentRowWithLink('RAM', build.ram),
      ...this.buildComponentRowWithLink('Storage', build.storage),
      ...this.buildComponentRowWithLink('GPU', build.gpu),
      ...this.buildComponentRowWithLink('PSU', build.psu),
      ...this.buildComponentRowWithLink('Case', build.case),
    ];

    autoTable(doc, {
      head: [['Component', 'Title', 'Price (грн)', 'Availability', 'Reviews']],
      body: rows,
      startY: 45,
      styles: { fontSize: 10, font: 'Roboto-Variable' },
      headStyles: {
        fillColor: [52, 152, 219],
        fontSize: 12,
        font: 'Roboto-Variable',
        fontStyle: 'normal',
      },
      alternateRowStyles: { fillColor: [245, 245, 245] },
      margin: { left: 14, right: 14 },
    });

    doc.save(`Build_${build.id}.pdf`);
  }

  private buildComponentRowWithLink(
    label: string,
    component: ComputerComponent
  ): any[] {
    const detailRow = [
      label,
      component?.title ?? 'N/A',
      component?.price ?? 0,
      component?.availability ?? 'N/A',
      component?.reviews ?? 'N/A',
    ];

    const linkRow = [
      {
        content: component?.link ?? 'N/A',
        colSpan: 5,
        styles: {
          textColor: component?.link ? [0, 0, 255] : [100, 100, 100],
          fontStyle: component?.link ? 'normal' : 'italic',
        },
      },
    ];

    return [detailRow, linkRow];
  }
}
