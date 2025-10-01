#!/usr/bin/env python3
import csv
import requests
import os
import re
from urllib.parse import urlparse

def extract_github_info(codelink):
    """Extract repository, file path, and commit from GitHub URL"""
    pattern = r'https://github\.com/([^/]+/[^/]+)/blob/([^/]+)/(.+?)(?:#.*)?$'
    match = re.match(pattern, codelink)
    if match:
        repo = match.group(1)
        commit = match.group(2)
        file_path = match.group(3)
        return repo, commit, file_path
    return None, None, None

def download_file(repo, commit, file_path, output_dir):
    """Download file from GitHub raw URL"""
    raw_url = f"https://raw.githubusercontent.com/{repo}/{commit}/{file_path}"
    
    # Create safe filename
    safe_filename = file_path.replace('/', '_').replace('\\', '_')
    repo_safe = repo.replace('/', '_')
    output_file = os.path.join(output_dir, f"{repo_safe}_{safe_filename}")
    
    try:
        response = requests.get(raw_url, timeout=30)
        response.raise_for_status()
        
        with open(output_file, 'w', encoding='utf-8') as f:
            f.write(response.text)
        
        print(f"Downloaded: {output_file}")
        return True
    except Exception as e:
        print(f"Failed to download {raw_url}: {e}")
        return False

def main():
    csv_file = '/Users/jattipre/Workspace/DarwinMCP/darwin_findings.csv'
    output_dir = 'path-traversal'
    
    # Ensure output directory exists
    os.makedirs(output_dir, exist_ok=True)
    
    unique_files = set()
    downloaded_count = 0
    
    with open(csv_file, 'r', encoding='utf-8') as f:
        reader = csv.DictReader(f)
        
        for i, row in enumerate(reader):
            if i >= 50:  # Only process first 50 rows
                break
                
            codelink = row.get('codelink', '')
            if not codelink:
                continue
                
            repo, commit, file_path = extract_github_info(codelink)
            if not all([repo, commit, file_path]):
                continue
                
            # Create unique identifier for the file
            file_id = f"{repo}:{file_path}"
            
            if file_id not in unique_files:
                unique_files.add(file_id)
                
                if download_file(repo, commit, file_path, output_dir):
                    downloaded_count += 1
                    
                if downloaded_count >= 50:  # Limit to 50 unique files
                    break
    
    print(f"\nSummary:")
    print(f"Unique files identified: {len(unique_files)}")
    print(f"Files successfully downloaded: {downloaded_count}")

if __name__ == "__main__":
    main()
