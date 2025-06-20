import { Injectable } from '@angular/core';
import { supabase } from './supabase-client';

@Injectable({
  providedIn: 'root'
})
export class SupabaseUpload {
async uploadVisitorPhoto(base64Image: string): Promise<string> {
    const fileName = `visitor_${Date.now()}.png`;
    const { data, error } = await supabase.storage
      .from('visitor-photos')
      .upload(fileName, this.dataURLtoBlob(base64Image), {
        contentType: 'image/png',
        upsert: false
      });

    if (error) throw error;

    const { data: publicUrlData } = supabase
      .storage
      .from('visitor-photos')
      .getPublicUrl(fileName);

    return publicUrlData?.publicUrl || '';
  }
async uploadHostPhoto(base64Image: string): Promise<string> {
    const fileName = `host_${Date.now()}.png`;
    const { data, error } = await supabase.storage
      .from('host-photos')
      .upload(fileName, this.dataURLtoBlob(base64Image), {
        contentType: 'image/png',
        upsert: false
      });

    if (error) throw error;

    const { data: publicUrlData } = supabase
      .storage
      .from('host-photos')
      .getPublicUrl(fileName);

    return publicUrlData?.publicUrl || '';
  }

  private dataURLtoBlob(dataURL: string): Blob {
    const byteString = atob(dataURL.split(',')[1]);
    const mimeString = dataURL.split(',')[0].split(':')[1].split(';')[0];
    const ab = new ArrayBuffer(byteString.length);
    const ia = new Uint8Array(ab);
    for (let i = 0; i < byteString.length; i++) {
      ia[i] = byteString.charCodeAt(i);
    }
    return new Blob([ab], { type: mimeString });
  }
}