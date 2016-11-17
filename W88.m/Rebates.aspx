<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Site.master" AutoEventWireup="true" CodeFile="Rebates.aspx.cs" Inherits="Rebates" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="ui-content rebates" role="main">
    	<form action="" class="form">
	    	<ul class="list fixed-tablet-size">
		    	<li class="item item-select">
		    		<label >Period</label>
		    		<div class="ui-select">
		    			<div class="ui-btn ui-icon-carat-d ui-btn-icon-right ui-shadow">
		    				<span>2016 Week 44 (Current Week)</span>
				    	</div>
				    </div>
				    <p class="text-center"><small>Monday (2016/10/31) - Sunday (2016/11/06)</small> </p>
		    	</li>
	    	</ul>
			<div class="rebates-collapse-group">
				<h2>Sportsbook</h2>
		    	<div data-role="collapsible" data-inset="false">
				    <h3>a-Sports</h3>
				    <table class="rebates-table">
						<tbody>
							<tr>
								<td>Total Eligible Bets</td>
								<td>0.00</td>
							</tr>
							<tr>
								<td>Rebate (%)</td>
								<td>0.00</td>
							</tr>
							<tr>
								<td>Rebate Amount</td>
								<td>0.00</td>
							</tr>
							<tr>
								<td colspan="2">
									<input type="button" name="btnSubmit" value="Instant Claim" id="rebatesClaim" class="button-blue" data-theme="b">
								</td>
							</tr>
						</tbody>
					</table>
				</div>
				<div data-role="collapsible" data-inset="false">
				    <h3>e-Sports</h3>
				    <table class="rebates-table">
						<tbody>
							<tr>
								<td>Total Eligible Bets</td>
								<td>0.00</td>
							</tr>
							<tr>
								<td>Rebate (%)</td>
								<td>0.00</td>
							</tr>
							<tr>
								<td>Rebate Amount</td>
								<td>0.00</td>
							</tr>
							<tr>
								<td colspan="2">
									<input type="button" name="btnSubmit" value="Instant Claim" id="" class="button-blue" data-theme="b">
								</td>
							</tr>
						</tbody>
					</table>
				</div>
				<div data-role="collapsible" data-inset="false">
				    <h3>i-Sports</h3>
				    <table class="rebates-table">
						<tbody>
							<tr>
								<td>Total Eligible Bets</td>
								<td>0.00</td>
							</tr>
							<tr>
								<td>Rebate (%)</td>
								<td>0.00</td>
							</tr>
							<tr>
								<td>Rebate Amount</td>
								<td>0.00</td>
							</tr>
							<tr>
								<td colspan="2">
									<input type="button" name="btnSubmit" value="Instant Claim" id="" class="button-blue" data-theme="b">
								</td>
							</tr>
						</tbody>
					</table>
				</div>
			</div>
			<div class="rebates-collapse-group">
				<h2>Slots</h2>
		    	<div data-role="collapsible" data-inset="false">
				    <h3>Bravado</h3>
				    <table class="rebates-table">
						<tbody>
							<tr>
								<td>Total Eligible Bets</td>
								<td>0.00</td>
							</tr>
							<tr>
								<td>Rebate (%)</td>
								<td>0.00</td>
							</tr>
							<tr>
								<td>Rebate Amount</td>
								<td>0.00</td>
							</tr>
							<tr>
								<td colspan="2">
									<input type="button" name="btnSubmit" value="Instant Claim" id="" class="button-blue" data-theme="b">
								</td>
							</tr>
						</tbody>
					</table>
				</div>
				<div data-role="collapsible" data-inset="false">
				    <h3>e-Sports</h3>
				    <table class="rebates-table">
						<tbody>
							<tr>
								<td>Total Eligible Bets</td>
								<td>0.00</td>
							</tr>
							<tr>
								<td>Rebate (%)</td>
								<td>0.00</td>
							</tr>
							<tr>
								<td>Rebate Amount</td>
								<td>0.00</td>
							</tr>
							<tr>
								<td colspan="2">
									<input type="button" name="btnSubmit" value="Instant Claim" id="" class="button-blue" data-theme="b">
								</td>
							</tr>
						</tbody>
					</table>
				</div>
				<div data-role="collapsible" data-inset="false">
				    <h3>i-Sports</h3>
				    <table class="rebates-table">
						<tbody>
							<tr>
								<td>Total Eligible Bets</td>
								<td>0.00</td>
							</tr>
							<tr>
								<td>Rebate (%)</td>
								<td>0.00</td>
							</tr>
							<tr>
								<td>Rebate Amount</td>
								<td>0.00</td>
							</tr>
							<tr>
								<td colspan="2">
									<input type="button" name="btnSubmit" value="Instant Claim" id="" class="button-blue" data-theme="b">
								</td>
							</tr>
						</tbody>
					</table>
				</div>
			</div>

			<div class="rebates-disclaimer">
				<p><small>Disclaimer: Min. Rebate Amount: USD 1.00 Rebate Amount displayed may deviate from the actual rebate paid out. Refer to "Promotion Claim" history for the actual payout.</small></p>
			</div>
    	</form>
    </div>

    <div id="rebatesModal" data-role="popup" data-overlay-theme="b" data-theme="b" data-history="false">
        <a href="#" id="closeModal" class="close close-enhanced">&times;</a>
        <div class="padding">
   			<div class="padding rebates-modal-content">
   				<div class="text-center">
	   				<h4>Rebate Summary</h4>
	   				<p>Monday (2016/10/31) - Sunday (2016/11/06) </p>

	   				<!-- Display if successful -->
	   				<!-- 
		   				<h4>Congratulations</h4>
		   				<p>Your instant claim of <span class="text-blue">RMB 6.52</span> has been successfully credited into your Main Wallet.</p> 
		   			-->
   				</div>
			    <table class="rebates-table">
					<tbody>
						<tr>
							<td>Total Eligible Bets</td>
							<td>0.00</td>
						</tr>
						<tr>
							<td>Rebate (%)</td>
							<td>0.00</td>
						</tr>
						<tr>
							<td>Rebate Amount</td>
							<td>0.00</td>
						</tr>
						<tr>
							<td colspan="2">
								<input type="button" name="" value="Instant Claim" id="" class="button-blue" data-theme="b">
							</td>
						</tr>
					</tbody>
				</table>

				<p><small>*Min. Rebate AMount: RMB 6.00 *All rebate being instant claims are deposited into the main wallet within 30mins and should this failed, please contact our customer service *All rebate are not subjected to tollover and may be withdrawn</small></p>
   			</div>
        	
        </div>
    </div>

    <script>
		$( "#rebatesClaim" ).bind( "click", function() {
			$('#rebatesModal').popup('open');
		});   
		$( "#closeModal" ).bind( "click", function() {
			$('#rebatesModal').popup('close');
		});    
	</script>

</asp:Content>

