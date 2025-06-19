import { TestBed } from '@angular/core/testing';

import { SupabaseUpload } from './supabase-upload';

describe('SupabaseUpload', () => {
  let service: SupabaseUpload;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SupabaseUpload);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
