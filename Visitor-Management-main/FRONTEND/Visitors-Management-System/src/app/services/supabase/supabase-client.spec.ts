import { TestBed } from '@angular/core/testing';

import { SupabaseClient } from './supabase-client';

describe('SupabaseClient', () => {
  let service: SupabaseClient;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SupabaseClient);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
