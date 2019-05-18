## Report Manager - an automatic reporting system

# Set up:
export DEFAULT_AWS_PROFILE=<<AWS_PROFILE>>

# Run WebApp:
cd angular
./run_angular.sh

# Run Report Manager
cd servers/report-manager
./run_report_manager.sh

# Test Locally
http://localhost:4200/reports

# Deployment via CodePipeline

